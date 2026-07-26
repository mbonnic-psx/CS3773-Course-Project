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
| DB1 | Design database schema (users, products, cart, orders…)  | High     | Done   | —   |
| DB2 | Create tables + relationships                            | High     | Done   | —   |
| DB3 | Seed product catalog with grocery items + images         | High     | Done   | —   |
| DB4 | Seed discount codes                                      | Med      | Done   | —   |
| DB5 | Package schema + seed as `schema.sql` for import         | High     | Done   | —   |

---

| ID   | Backend & PHP                                   | Priority | Status | Who |
| ---- | --------------------------------------- | -------- | ------ | --- |
| BE1  | Database connection (`db.php`)          | High     | Done   | —   |
| BE2  | Register user                           | High     | Done   | —   |
| BE3  | Log in user                             | High     | Done   | —   |
| BE4  | Add / get / delete addresses            | High     | Done   | —   |
| BE5  | Get products (with search + sort)       | High     | Done   | —   |
| BE6  | Add / get / update / delete cart items  | High     | Done   | —   |
| BE7  | Validate + apply discount code          | Med      | Done   | —   |
| BE8  | Place order + save order items          | High     | Done   | —   |
| BE9  | Get order history                       | Med      | Done   | —   |
| BE10 | Modify customer information endpoint    | Med      | To Do  | —   |

---

| ID  | Frontend Unity Screens                                                    | Priority | Status | Who |
| --- | ------------------------------------------------------- | -------- | ------ | --- |
| FE1 | Server request layer (`UnityWebRequest` + image loader) | High     | Done   | —   |
| FE2 | Login + Register screens                                | High     | Done   | —   |
| FE3 | Product catalog (search, sort, images)                  | High     | Done   | —   |
| FE4 | Shopping cart screen (view, add, remove, quantity)      | High     | Done   | —   |
| FE5 | Address management screen                               | High     | Done   | —   |
| FE6 | Checkout screen (subtotal, tax, discount, delivery)     | High     | Done   | —   |
| FE7 | Order history screen (sort by date / amount)            | Med      | Done   | —   |
| FE8 | Scene navigation + user session handling                | High     | Done   | —   |

---

| ID  | Integration, Testing, Delivery                                                    | Priority | Status      | Who     |
| --- | ------------------------------------------------------- | -------- | ----------- | ------- |
| IN1 | Merge frontend, backend, and database layers            | High     | Done        | — |
| IN2 | End-to-end test of full flow (register → order history) | High     | Done        | —       |
| IN3 | MAMP setup guide (`SETUP.md`) + updated README          | High     | Done        | — |
| IN4 | Organize repo (`CustomerPortal_MAMP`, docs folder)      | Med      | Done        | — |
| IN5 | Build Windows executable                                | High     | In Progress | —       |
| IN6 | Publish GitHub Release with downloadable build          | High     | In Progress | —       |
| IN7 | Unit tests (Unity Test Framework)                       | High     | To Do       | —       |
| IN8 | Workload Distribution Report (`WORKLOAD.md`)            | High     | To Do       | —       |
| IN9 | Prepare demo — each member presents their part          | High     | To Do       | All     |

---
