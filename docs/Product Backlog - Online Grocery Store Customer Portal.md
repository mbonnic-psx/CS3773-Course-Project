A prioritized to-do list for the project. Items at the **top** are built first. Update this file as work gets done (To Do → In Progress → Done).

**Priority:** High / Med / Low **Status:** To Do / In Progress / Done **Who:** Member Name

---

## Tech Stack (Decided)
 
- **Unity (C#)** — the customer portal app / front end
- **PHP** — server-side backend; handles requests from Unity and talks to the database
- **MySQL** — database storing customers, products, carts, orders, etc.
Unity sends requests to the PHP backend (via `UnityWebRequest`), and PHP reads/writes the MySQL database.

---

| ID  | User Story                                                              | Priority | Status | Who |
| --- | ----------------------------------------------------------------------- | -------- | ------ | --- |
| 1   | Write User Stories for Every Feture                          | High     | Done  | Aaron/Carlos  |
| 2 | Commit User Stories | High | Done | Carlos |


---

| ID  | Design Documents & Diagram | Priority | Status | Who   |
| --- | -------------------------- | -------- | ------ | ----- |
| 1   | Finalize Class Diagram     | High     | Done   | Bryan |
| 2   | Finalize State Diagram     | High     | Done | Bryan |
| 3   | Commit both Diagrams       | High     | Done  | Bryan    |

---

| ID  | Repo & Process             | Priority | Status | Who     |
| --- | -------------------------- | -------- | ------ | ------- |
| 1   | Create ReadMe for Project  | High     | Done   | Matthew |
| 2   | Create Backlog for Project | High     | Done   | Matthew |

---

| ID  | Database                                                    | Priority | Status | Who |
| --- | -------------------------------------------------------- | -------- | ------ | --- |
| 1 | Design database schema (users, products, cart, orders…)  | High     | Done   | Bryan |
| 2 | Create tables + relationships                            | High     | Done   | Bryan |
| 3 | Seed product catalog with grocery items + images         | High     | Done   | —   |
| 4 | Seed discount codes                                      | Med      | Done   | —   |
| 5 | Package schema + seed as `schema.sql` for import         | High     | Done   | —   |

---

| ID   | Backend & PHP                                   | Priority | Status | Who |
| ---- | --------------------------------------- | -------- | ------ | --- |
| 1  | Database connection (`db.php`)          | High     | Done   | Bryan |
| 2  | Register user                           | High     | Done   | Bryan |
| 3  | Log in user                             | High     | Done   | Bryan |
| 4  | Add / get / delete addresses            | High     | Done   | Bryan |
| 5  | Get products (with search + sort)       | High     | Done   | Bryan |
| 6  | Add / get / update / delete cart items  | High     | Done   | Bryan |
| 7  | Validate + apply discount code          | Med      | Done   | Bryan |
| 8  | Place order + save order items          | High     | Done   | Bryan |
| 9  | Get order history                       | Med      | Done   | Bryan |
| 10 | Modify customer information endpoint    | Med      | To Do  | Bryan |

---

| ID  | Frontend Unity Screens                                                    | Priority | Status | Who |
| --- | ------------------------------------------------------- | -------- | ------ | --- |
| 1 | Server request layer (`UnityWebRequest` + image loader) | High     | Done   | —   |
| 2 | Login + Register screens                                | High     | Done   | —   |
| 3 | Product catalog (search, sort, images)                  | High     | Done   | —   |
| 4 | Shopping cart screen (view, add, remove, quantity)      | High     | Done   | —   |
| 5 | Address management screen                               | High     | Done   | —   |
| 6 | Checkout screen (subtotal, tax, discount, delivery)     | High     | Done   | —   |
| 7 | Order history screen (sort by date / amount)            | Med      | Done   | —   |
| 8 | Scene navigation + user session handling                | High     | Done   | —   |

---

| ID  | Integration, Testing, Delivery                                                    | Priority | Status      | Who     |
| --- | ------------------------------------------------------- | -------- | ----------- | ------- |
| 1 | Merge frontend, backend, and database layers            | High     | Done        | — |
| 2 | End-to-end test of full flow (register → order history) | High     | Done        | —       |
| 3 | MAMP setup guide (`SETUP.md`) + updated README          | High     | Done        | — |
| 4 | Organize repo (`CustomerPortal_MAMP`, docs folder)      | Med      | Done        | — |
| 5 | Build Windows executable                                | High     | In Progress | —       |
| 6 | Publish GitHub Release with downloadable build          | High     | In Progress | —       |
| 7 | Unit tests (Unity Test Framework)                       | High     | To Do       | —       |
| 8 | Prepare demo           | High     | To Do       | All     |

---
