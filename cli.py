from susu import SusuGroup, format_money


def main() -> None:
    print("Susu Group Tracker")

    while True:
        try:
            amount = input("Weekly contribution in GHS: ")
            group = SusuGroup(amount)
            break
        except ValueError as error:
            print(f"Error: {error}")

    while True:
        print("\n1. Add member")
        print("2. Record contribution")
        print("3. Close current week")
        print("4. Show report")
        print("5. Exit")

        choice = input("Choose an option: ").strip()

        try:
            if choice == "1":
                name = input("Member name: ")
                group.add_member(name)
                print("Member added.")

            elif choice == "2":
                name = input("Member name: ")
                week = int(input("Week number: "))
                amount = input("Payment amount in GHS: ")

                group.record_contribution(name, week, amount)
                print("Contribution recorded.")

            elif choice == "3":
                result = group.close_week()

                if not result["closed"]:
                    print("Week remains open. No payout.")
                    for member, owed in result["outstanding"].items():
                        print(
                            f"{member} owes GHS {format_money(owed)}"
                        )
                else:
                    payout = result["payout"]
                    print(
                        f"Week {payout.week} closed. "
                        f"{payout.member} receives "
                        f"GHS {format_money(payout.amount)}."
                    )

                    if group.completed:
                        print("Savings cycle complete.")
                    else:
                        print(f"Week {group.current_week} begins.")

            elif choice == "4":
                print()
                print(group.report())

            elif choice == "5":
                print("Goodbye.")
                break

            else:
                print("Choose an option from 1 to 5.")

        except ValueError as error:
            print(f"Error: {error}")


if __name__ == "__main__":
    try:
        main()
    except (EOFError, KeyboardInterrupt):
        print("\nGoodbye.")