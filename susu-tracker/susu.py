import re
from dataclasses import dataclass


def parse_money(value: str) -> int:
    """Convert GHS text to integer pesewas without rounding."""
    value = value.strip()

    if not re.fullmatch(r"[0-9]+(?:\.[0-9]{1,2})?", value):
        raise ValueError(
            "Enter a valid amount with at most two decimal places."
        )

    whole, _, fraction = value.partition(".")
    return int(whole) * 100 + int(fraction.ljust(2, "0"))


def format_money(pesewas: int) -> str:
    return f"{pesewas // 100}.{pesewas % 100:02d}"


@dataclass(frozen=True)
class Payout:
    week: int
    member: str
    amount: int


class SusuGroup:
    def __init__(self, weekly_amount: str):
        amount = parse_money(weekly_amount)

        if amount <= 0:
            raise ValueError("Weekly contribution must be positive.")

        self.weekly_amount = amount
        self.members: list[str] = []
        self.current_week = 1
        self.payments: dict[int, dict[str, int]] = {}
        self.payout_history: list[Payout] = []
        self.membership_locked = False
        self.completed = False

    def add_member(self, name: str) -> None:
        if self.membership_locked:
            raise ValueError(
                "Cannot add members after contributions have started."
            )

        name = name.strip()

        if not name:
            raise ValueError("Member name cannot be blank.")

        if any(
            member.casefold() == name.casefold()
            for member in self.members
        ):
            raise ValueError("Member already exists.")

        self.members.append(name)

    def _find_member(self, name: str) -> str:
        name = name.strip()

        for member in self.members:
            if member.casefold() == name.casefold():
                return member

        raise ValueError("Unknown member.")

    def record_contribution(
        self, name: str, week: int, amount: str
    ) -> None:
        if self.completed:
            raise ValueError("This savings cycle is complete.")

        if not self.members:
            raise ValueError("Add members first.")

        if type(week) is not int or week != self.current_week:
            raise ValueError(
                f"Only payments for week {self.current_week} are accepted."
            )

        member = self._find_member(name)
        payment = parse_money(amount)

        if payment <= 0:
            raise ValueError("Payment must be positive.")

        already_paid = self.payments.get(week, {}).get(member, 0)
        outstanding = self.weekly_amount - already_paid

        if payment > outstanding:
            raise ValueError(
                f"{member} only owes GHS {format_money(outstanding)}."
            )

        # Update state only after validation succeeds.
        self.payments.setdefault(week, {})[member] = (
            already_paid + payment
        )
        self.membership_locked = True

    def balances(self) -> dict[str, int]:
        week_payments = self.payments.get(self.current_week, {})

        return {
            member: self.weekly_amount - week_payments.get(member, 0)
            for member in self.members
        }

    def close_week(self) -> dict:
        if self.completed:
            raise ValueError("This savings cycle is complete.")

        if not self.members:
            raise ValueError("Add members first.")

        outstanding = {
            member: owed
            for member, owed in self.balances().items()
            if owed > 0
        }

        if outstanding:
            return {
                "closed": False,
                "outstanding": outstanding,
            }

        payout = Payout(
            week=self.current_week,
            member=self.members[self.current_week - 1],
            amount=self.weekly_amount * len(self.members),
        )
        self.payout_history.append(payout)

        if self.current_week == len(self.members):
            self.completed = True
        else:
            self.current_week += 1

        return {
            "closed": True,
            "payout": payout,
        }

    def report(self) -> str:
        lines = [
            f"Weekly contribution: GHS {format_money(self.weekly_amount)}",
            f"Current week: {self.current_week}",
            f"Status: {'Complete' if self.completed else 'Active'}",
            "",
            "Member balances:",
        ]

        if not self.members:
            lines.append("No members yet.")

        for member, owed in self.balances().items():
            if owed == 0:
                lines.append(f"- {member}: paid in full")
            else:
                lines.append(
                    f"- {member}: owes GHS {format_money(owed)}"
                )

        if self.members and not self.completed:
            lines.append(
                f"\nNext recipient: {self.members[self.current_week - 1]}"
            )

        lines.append("\nPayout history:")

        if not self.payout_history:
            lines.append("No payouts yet.")

        for payout in self.payout_history:
            lines.append(
                f"- Week {payout.week}: {payout.member} received "
                f"GHS {format_money(payout.amount)}"
            )

        return "\n".join(lines)