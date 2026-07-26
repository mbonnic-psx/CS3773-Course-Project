# CS3773 Course Project — Online Grocery Store Customer Portal

> An Online Grocery System **Customer Portal**: the interface system that shop customers use to browse products, manage their accounts, and place orders with checkout and delivery options.
>
> Built with **Unity 6** (C# frontend) + **PHP / MySQL** backend for **CS3773 Software Engineering**.

> ▶️ **Just want to try it?** You don't need Unity or the source — see **[Download & Play the Build](#download--play-the-build)** below.

---

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
| Frontend UI | Unity UI (uGUI) | HTTP via `UnityWebRequest` |
| Backend | **PHP** | REST-style endpoints in `CustomerPortal_MAMP/` |
| Database | **MySQL / MariaDB** | database name `CustomerPortal` |
| Local server | **MAMP** | Apache + MySQL + phpMyAdmin |
| Testing | Unity Test Framework | `1.4.5` |
| Version Control | Git + GitHub | - |

---

## How It Works

```
Unity build (.exe)  ──HTTP──▶  Apache/PHP (CustomerPortal)  ──SQL──▶  MySQL (CustomerPortal)
     frontend                       backend endpoints                 products, users, orders…
```

The game talks to the PHP endpoints over HTTP at `http://localhost/CustomerPortal`. The backend **must be running before** the game will load products, log in, or place orders.

---

## Getting Started

There are two ways to run this project. **Both require the backend (MAMP) to be running first.**

### Download & Play the Build
Best for teammates, graders, or a quick demo — **no Unity or source needed.**

1. **Download** the latest **`OnlineGroceryStore-Windows.zip`** from the repo's **[Releases](https://github.com/mbonnic-psx/CS3773-Course-Project/releases)** page and unzip it somewhere easy. Keep `OnlineGroceryStore.exe`, `OnlineGroceryStore_Data/`, and `UnityPlayer.dll` **together in the same folder**.
2. **Set up the backend** with **[SETUP.md](SETUP.md)** (one time): install MAMP on port 80, copy `CustomerPortal_MAMP` into `htdocs` and rename it to `CustomerPortal`, then import `schema.sql` in phpMyAdmin.
3. **Run it:** start MAMP (**Start Servers** → green lights), then double-click **`OnlineGroceryStore.exe`**. Register → log in → the catalog loads with images.

> ✔️ Quick check before launching: <http://localhost/CustomerPortal/getProducts.php?search=> should return product JSON. If a "Windows protected your PC" popup appears, click **More info → Run anyway** (the build is unsigned).

### Run from Source (Unity)
Best for development.
1. Open the **`CS3773-Course Project/`** folder in **Unity 6 (`6000.0.32f1`)**.
2. Set up the backend using **[SETUP.md](SETUP.md)**.
3. Open the `LoginScene` and press **Play**.

> ⚠️ **Folder name note:** the backend lives in `CustomerPortal_MAMP/`, but the app calls `http://localhost/CustomerPortal`. When you copy it into MAMP's `htdocs`, **rename the copy to `CustomerPortal`** so the URL matches (no code change needed). Full steps are in [SETUP.md](SETUP.md).

### Publishing a new build to Releases (for the team)
So people can download without the source:
1. Unity → **File → Build Settings → Windows → Build**, then **zip the entire build output folder** (`.exe` + `_Data` + `UnityPlayer.dll`) as `OnlineGroceryStore-Windows.zip`.
2. GitHub → repo → **Releases → Draft a new release** → tag it (e.g. `v1.0-demo`) → **drag the zip into "Attach binaries"** → **Publish release**.
3. Build with `ServerRequest.SERVER_URL = "http://localhost/CustomerPortal"` (the default) — the URL can't be changed after building.

---

## Repository Structure

```
CS3773-Course-Project/
├── README.md
├── SETUP.md                      # MAMP + database setup (backend)
├── .gitignore
│
├── CS3773-Course Project/        # Unity 6 project — customer portal frontend (C#)
│   ├── Assets/
│   ├── Packages/
│   └── ProjectSettings/
│
├── CustomerPortal_MAMP/          # PHP backend — copy to htdocs (rename to CustomerPortal)
│   ├── *.php                     # REST endpoints (login, products, cart, orders, …)
│   ├── db.php                    # DB connection (root/root/CustomerPortal)
│   ├── schema.sql                # DB schema + seed data — import in phpMyAdmin
│   └── images/                   # product images
│
└── docs/
    ├── UML-Draft.png                     # class diagram
    ├── OrderClassStateDiagram.png        # Order state diagram
    ├── User Stories.md                   # user stories + test cases
    └── Product Backlog - Online Grocery Store Customer Portal.md
```

> 🧹 Cleanup note: there are loose `.cs` and `.php` files at the repo root left over from earlier commits. The authoritative copies live inside `CS3773-Course Project/Assets/` and `CustomerPortal_MAMP/`; the root duplicates can be removed.

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
- [x] Testable user stories + test cases (`docs/User Stories.md`)
- [x] Class diagram (`docs/UML-Draft.png`)
- [x] State diagram (`docs/OrderClassStateDiagram.png`)
- [x] Product backlog committed (`docs/Product Backlog … .md`)
- [x] Database approach decided (Unity → PHP → MySQL via MAMP)

### 🔜 Due July 30th — Final Delivery
- [ ] Source code implementing all features in the spec
- [ ] Database integrated and preloaded with grocery items
- [ ] Account registration + login
- [ ] Add / manage addresses
- [ ] Browse + search (sort by price, sort by availability; show price, picture, name)
- [ ] Shopping cart (view, add, remove)
- [ ] Checkout: 8.25% tax, discount codes, multiple delivery options, order summary + place order
- [ ] Order history (sort by date, sort by dollar amount)
- [ ] Unit tests included (Unity Test Framework)
- [ ] User stories updated if the design changed
- [ ] Final class diagram + state diagram committed
- [ ] Workload Distribution Report
- [ ] Clean version history + product backlog change history

### 🎤 Presentation & Demo — July 28th & 30th
- [ ] Every member presents part of the work
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
