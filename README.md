# CS3773 Course Project — Online Grocery Store Customer Portal

> An Online Grocery System **Customer Portal**: the interface system that shop customers use to browse products, manage their accounts, and place orders with checkout and delivery options.
>
> Built with **Unity 6** for **CS3773 Software Engineering**.


## Project Overview

The Customer Portal of the Online Grocery Store allows users to browse products, manage personal accounts, and place orders with different checkout and delivery options. It features a user-friendly interface with search, filtering, cart management, and integrated checkout.

The system is **database-driven** and lets customers:
- Register and manage personal accounts
- Browse and search for grocery items
- Add items to a virtual cart and place orders
- Choose delivery options and manage addresses
- View their order history

---

## Features

**Accounts & Addresses**
- Register an account and log in
- Add and manage delivery addresses

**Browse & Search**
- Search items by name / description
- Sort by price
- Sort by availability
- Show item price, picture, and name

**Shopping Cart**
- View items currently in the cart
- Add and remove items

**Checkout**
- Calculate tax at an **8.25%** rate
- Apply discount codes
- Offer more than one delivery type
- Show an order summary and place the order

**Order History**
- Sort by order date
- Sort by order size (dollar amount)

---

## Tech Stack

| Layer | Choice | Version / Notes |
|---|---|---|
| Engine | **Unity** | `6000.0.32f1` (Unity 6) |
| Language | **C#** | Unity scripting |
| Rendering | Universal Render Pipeline (URP) | `17.0.3` |
| UI | Unity UI (uGUI) + UI Toolkit (UIElements) | `2.0.0` |
| Testing | **Unity Test Framework** | `1.4.5` (unit tests are a required deliverable) |
| Version Control | Git + GitHub | - |
| IDE | Visual Studio / JetBrains Rider / VS Code | - |
| Database | Unity + PHP + MySQL | - |

---

## System Design (UML)

![UML](https://github.com/mbonnic-psx/CS3773-Course-Project/blob/main/docs/UML-Draft.png)

### Class Summary

| Class | Key Attributes | Key Methods | Relationships |
|---|---|---|---|
| **Customer** | customerID, firstName, lastName, email, password | register(), login(), updateProfile() | 1→1 ShoppingCart · 1→\* Address · 1→\* Order |
| **Address** | addressID, street, city, state, zipCode | getAddress(), updateAddress() | belongs to a Customer |
| **ShoppingCart** | cartID, totalPrice | addItem(), removeItem(), calculateTotal() | 1→\* CartItem |
| **CartItem** | quantity | — | \*→1 Product |
| **Product** | productID, name, description, price, quantityAvailable, imageURL | search(), sortByPrice(), sortByAvailability() | referenced by CartItem & OrderItem |
| **Order** | orderID, orderDate, totalAmount, tax, discount, deliveryType, status | placeOrder(), calculateTax() | 1→\* OrderItem · \*→\* DiscountCode · \*→1 DeliveryOption |
| **OrderItem** | quantity, subtotal | — | \*→1 Product |
| **DiscountCode** | code, percentOff, expirationDate | — | applied to Order |
| **DeliveryOption** | deliveryID, type, deliveryFee | — | chosen on Order |

### State Diagram

![Order State Diagram](https://github.com/mbonnic-psx/CS3773-Course-Project/blob/main/docs/OrderClassStateDiagram.png)

---

## Repository Structure

```
CS3773-Course-Project/
├── README.md
├── PRODUCT_BACKLOG.md           # prioritized backlog
├── USER_STORIES.md              # user stories + test cases
├── WORKLOAD.md                  # workload distribution report
├── .gitignore
│
├── docs/                        # diagrams (class diagram, state diagram)
│   └── UML-Draft.png
|   └── OrderClassStateDiagram.png
|   └── Product Backlog - Online Grocery Store Customer Portal.md
|   └── User Stories
│
├── client/                      # Unity front end — the customer portal (C#)   ← exists
│   ├── Assets/
│   ├── Packages/
│   └── ProjectSettings/
│
├── server/                      # PHP backend — to be added later
│
└── database/                    # MySQL schema + seed data — to be added later
```

---

## Getting Started

> 📌 Add how to run it when we start getting some progress on it 

---

## Deliverables & Checklist

The project is graded out of **40 points**:

| Component | Points |
|---|---|
| Testable User Stories | 10 |
| Design Documents & Diagrams | 5 |
| Source Code | 5 |
| Demo | 10 |
| Product Backlog & History | 10 |

### ✅ Due June 30th — Project Check-In
*(Not graded at this date — instructor gives feedback. Counts toward the final.)*

**Testable User Stories**
- [x] Write user stories covering every feature area (accounts, addresses, browse/search, cart, checkout, order history)
- [x] For **each** user story, write one or more natural-language test cases
- [x] Save them as a file in the repo (e.g. `USER_STORIES.md`)

**Design Documents & Diagrams**
- [x] Finalize the **class diagram** (UML-Draft.png is a solid start — confirm attributes/methods/multiplicities)
- [x] Create the **state diagram** for one important class (recommended: `Order`, using `status`)
- [x] Commit both diagrams to the repo (e.g. a `docs/` folder)

**Repo & Process**
- [x] Create the **product backlog** as a file in the repo (`Product Backlog - Online Grocery Store Customer Portal.md`)
- [x] Confirm all group members are committing (version history shows collaboration)
- [x] **Decide on the database** approach and note it here + in the backlog

### 🔜 Due July 30th — Final Delivery
- [ ] **Source code** implementing all features in the spec
- [ ] **Database** integrated and preloaded with grocery items
- [ ] Account registration + login
- [ ] Add / manage addresses
- [ ] Browse + search (sort by price, sort by availability; show price, picture, name)
- [ ] Shopping cart (view, add, remove)
- [ ] Checkout: 8.25% tax, discount codes, multiple delivery options, order summary + place order
- [ ] Order history (sort by date, sort by dollar amount)
- [ ] **Unit tests** included in the delivered code (Unity Test Framework)
- [ ] **User stories** updated if the design changed
- [ ] Final **class diagram** + **state diagram** committed
- [ ] **Workload Distribution Report** (`WORKLOAD.md`) — work shared evenly across members
- [ ] Clean version history + product backlog with its change history in the repo

### 🎤 Presentation & Demo — July 28th & 30th
- [ ] **Every member** presents part of the work
- [ ] Live or recorded demo of the major features running
- [ ] Discuss problems met during development
- [ ] Keep to **12 min** presentation/demo + **3 min** Q&A

---

## Product Backlog

> [Product Backlog](https://github.com/mbonnic-psx/CS3773-Course-Project/blob/main/docs/Product%20Backlog%20-%20Online%20Grocery%20Store%20Customer%20Portal.md)

---

## Team & Workload

| Member | GitHub | Responsibilities |
|---|---|---|
| Matthew Bonnichsen | [@mbonnic-psx](https://github.com/mbonnic-psx) | _TBD_ |
| Bryan Banuelos | [@BryanBanuelos](https://github.com/BryanBanuelos) | _TBD_ |
| Aaron Garza | [@Aaronc07](https://github.com/Aaronc07) | _TBD_ |
| Carlos Patiño | [@Vily3](https://github.com/Vily3) | _TBD_ |

---

## Course Info

- **Course:** CS3773 — Software Engineering
- **Project Option:** Online Grocery System - Customer Portal
- **Final delivery:** July 30th · **Presentations:** July 28th & 30th
