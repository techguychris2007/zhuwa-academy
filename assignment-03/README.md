# Assignment 03 — Library Management System (C# / .NET 8)

A console-based library management system demonstrating object-oriented design
through encapsulation, inheritance, and polymorphism.

## Setup

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
cd assignment-03
dotnet build
```

## Running

```bash
dotnet run --project LibrarySystem
```

Five books and two members are pre-loaded so the system is usable immediately.

## Running tests

```bash
dotnet test LibrarySystem.sln
```

30 tests, all passing.

---

## Class design

```
Models/
  Book              — title, author, ISBN; CheckOut / Return with availability guard
  Member (abstract) — id, name, email; MaxBooks / LoanDays / MemberType abstract
  RegularMember     — 3 books, 14-day loan
  PremiumMember     — 6 books, 30-day loan
  BorrowingRecord   — links Book + Member; borrow date, due date, optional return date
Services/
  Library           — manages collections; enforces all business rules
Program.cs          — 10-option console menu, re-prompts on blank or invalid input
```

### OOP principles demonstrated

| Principle | Where |
|---|---|
| **Encapsulation** | `Book.IsAvailable` is set only via `CheckOut`/`Return`; `Member.ActiveLoans` only via `IncrementLoans`/`DecrementLoans`; all validation in constructors |
| **Inheritance** | `RegularMember` and `PremiumMember` both extend the abstract `Member` class |
| **Polymorphism** | `Library.BorrowBook` calls `member.LoanDays` — the correct value (14 or 30) is chosen at runtime based on the actual type |
| **Abstraction** | `Member` defines the contract (`MaxBooks`, `LoanDays`, `MemberType`) without caring which concrete type satisfies it |

### Borrowing rules

| Member type | Books allowed | Loan period |
|---|---|---|
| Regular | 3 | 14 days |
| Premium | 6 | 30 days |

### Validation

- Book: title, author, ISBN all required; duplicate ISBN rejected
- Member: name, email required; duplicate email rejected
- Borrow: book must exist and be available; member must exist and be under their limit
- Return: record must exist and must not already be returned

---

## Assumptions & Known Limitations

- Data is in-memory only — it is lost when the app exits. A future version could
  serialise to JSON on quit and reload on start.
- Overdue fines are not calculated; the system flags overdue loans but takes no
  further action.
- There is no concept of book copies — each ISBN is a single physical item.
- Member email is used as the unique key for duplicate detection (case-insensitive).
