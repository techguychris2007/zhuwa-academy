# Susu Group Tracker

A Python command-line program for managing one rotating savings group.

## Requirements

Python 3.10 or newer. No external packages required.

## Run

```powershell
python cli.py
```

## Run tests

```powershell
python -m unittest -v
```

## Features

- Create a group with a fixed weekly contribution in GHS.
- Add unique members in joining order.
- Record full or partial contributions.
- Close a fully funded week and pay the next member.
- Report outstanding balances and payout history.

## Assumptions

- Members can be added until the first valid contribution is recorded.
- Member names are trimmed and compared case-insensitively.
- Only the current week accepts contributions.
- At least one member is required before payments or closing a week.
- A cycle ends after every member has received one payout.
- After completion, further payments and week closures are rejected.
- A failed week closure leaves all state unchanged.
- Amounts must have at most two decimal places; extra precision is rejected.
- Data is stored in memory and is lost when the program exits.
- On completion, the current week remains the final week and the report
  shows the cycle as complete.

## Money handling

Money is stored as integer pesewas. For example, GHS 50.00 is stored
as 5000. Input is parsed directly from text without floating-point
arithmetic or rounding.

## Example

Create a group with a weekly contribution of GHS 50.00.
Add Ama, Kofi, and Esi in that order.

Week 1:
- Ama pays 50.00.
- Kofi pays 50.00.
- Esi pays 30.00.

Closing the week pays nothing and reports that Esi owes 20.00.

After Esi pays another 20.00, closing the week pays Ama 150.00.
Week 2 begins, with Kofi as the next recipient.

## Structure

- susu.py: savings-group rules and money handling.
- cli.py: command-line interaction.
- test_susu.py: automated tests.