import unittest
from copy import deepcopy

from susu import SusuGroup, parse_money, format_money


class MoneyTests(unittest.TestCase):
    def test_exact_conversion(self):
        self.assertEqual(parse_money("50.00"), 5000)
        self.assertEqual(parse_money("0.10"), 10)
        self.assertEqual(parse_money("30.5"), 3050)
        self.assertEqual(format_money(2050), "20.50")

    def test_invalid_money(self):
        for amount in ["1.005", "-5", "abc", "", "NaN"]:
            with self.subTest(amount=amount):
                with self.assertRaises(ValueError):
                    parse_money(amount)


class SusuGroupTests(unittest.TestCase):
    def setUp(self):
        self.group = SusuGroup("50.00")
        for member in ["Ama", "Kofi", "Esi"]:
            self.group.add_member(member)

    def pay_everyone(self):
        for member in self.group.members:
            self.group.record_contribution(
                member, self.group.current_week, "50.00"
            )

    def test_challenge_example(self):
        self.group.record_contribution("Ama", 1, "50")
        self.group.record_contribution("Kofi", 1, "50")
        self.group.record_contribution("Esi", 1, "30")

        before = deepcopy(self.group.__dict__)
        result = self.group.close_week()

        self.assertFalse(result["closed"])
        self.assertEqual(result["outstanding"], {"Esi": 2000})
        self.assertEqual(self.group.__dict__, before)

        self.group.record_contribution("Esi", 1, "20")
        result = self.group.close_week()

        self.assertTrue(result["closed"])
        self.assertEqual(result["payout"].member, "Ama")
        self.assertEqual(result["payout"].amount, 15000)
        self.assertEqual(self.group.current_week, 2)
        self.assertEqual(self.group.balances()["Kofi"], 5000)

    def test_duplicate_and_blank_members(self):
        for name in [" ama ", ""]:
            with self.subTest(name=name):
                with self.assertRaises(ValueError):
                    self.group.add_member(name)

    def test_invalid_weekly_amount(self):
        for amount in ["0", "-50", "1.005"]:
            with self.subTest(amount=amount):
                with self.assertRaises(ValueError):
                    SusuGroup(amount)

    def test_invalid_payments_leave_state_unchanged(self):
        cases = [
            ("Unknown", 1, "10"),
            ("Ama", 1, "0"),
            ("Ama", 1, "-10"),
            ("Ama", 1, "50.01"),
            ("Ama", 1, "1.005"),
            ("Ama", 0, "10"),
            ("Ama", 2, "10"),
        ]

        for name, week, amount in cases:
            with self.subTest(name=name, week=week, amount=amount):
                before = deepcopy(self.group.__dict__)

                with self.assertRaises(ValueError):
                    self.group.record_contribution(name, week, amount)

                self.assertEqual(self.group.__dict__, before)

    def test_partial_payments_and_overpayment(self):
        self.group.record_contribution("ama", 1, "30")
        self.group.record_contribution("Ama", 1, "20")

        self.assertEqual(self.group.balances()["Ama"], 0)

        with self.assertRaises(ValueError):
            self.group.record_contribution("Ama", 1, "0.01")

    def test_membership_locks_after_first_payment(self):
        self.group.record_contribution("Ama", 1, "1")

        with self.assertRaises(ValueError):
            self.group.add_member("Yaw")

    def test_empty_group(self):
        group = SusuGroup("50")

        with self.assertRaises(ValueError):
            group.close_week()

        with self.assertRaises(ValueError):
            group.record_contribution("Ama", 1, "50")

    def test_full_cycle(self):
        for expected_recipient in ["Ama", "Kofi", "Esi"]:
            self.pay_everyone()
            result = self.group.close_week()
            self.assertEqual(
                result["payout"].member, expected_recipient
            )

        self.assertTrue(self.group.completed)
        self.assertEqual(len(self.group.payout_history), 3)

        before = deepcopy(self.group.__dict__)

        with self.assertRaises(ValueError):
            self.group.close_week()

        with self.assertRaises(ValueError):
            self.group.record_contribution("Ama", 3, "1")

        self.assertEqual(self.group.__dict__, before)

    def test_small_amounts_are_exact(self):
        group = SusuGroup("0.10")

        for name in ["Ama", "Kofi", "Esi"]:
            group.add_member(name)

        for name in group.members:
            group.record_contribution(name, 1, "0.10")

        result = group.close_week()
        self.assertEqual(result["payout"].amount, 30)

    def test_report_does_not_change_state(self):
        self.group.record_contribution("Esi", 1, "30")
        before = deepcopy(self.group.__dict__)

        report = self.group.report()

        self.assertIn("Esi: owes GHS 20.00", report)
        self.assertEqual(self.group.__dict__, before)


if __name__ == "__main__":
    unittest.main()