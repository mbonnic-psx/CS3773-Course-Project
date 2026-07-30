# Workload Distribution Report

**Project:** Online Grocery Store Customer Portal
**Course:** CS3773 - Software Engineering
**Repository:** https://github.com/mbonnic-psx/CS3773-Course-Project
**Date:** 7/30/2026

---

## Team

| Member | GitHub | Primary Area of Ownership |
|---|---|---|
| Matthew Bonnichsen | [@mbonnic-psx](https://github.com/mbonnic-psx) | Repo & Documentation, Backend support |
| Bryan Banuelos | [@BryanBanuelos](https://github.com/BryanBanuelos) | Backend (PHP) & MySQL Database |
| Aaron Garza | [@Aaronc07](https://github.com/Aaronc07) | UI Design & Unity Frontend |
| Carlos Patiño | [@Vily3](https://github.com/Vily3) | QA & Testing, Frontend support |

---

## How the Work Was Divided

The team used a **horizontal layer split** across the four members, so each
person owned one layer of the system end to end:

- **Frontend (Unity/C#)** - all customer-facing screens
- **Backend (PHP)** - all REST endpoints
- **Database (MySQL)** - schema and seed data
- **Documentation & Testing** - repo, backlog, reports, QA

To keep the layers from blocking each other, we locked in a **shared schema
and API contract early** (agreeing on the MySQL tables and the request/response
shape of every endpoint before coding). This let the frontend, backend, and
database work proceed in parallel and integrate cleanly later.

As we were moving through the different stages of productions our roles seem to change and morph. It started with two frontend and two backend people and then towards the end Matthew and Carlos broke off from there respected roles and filled in other areas. Matthew handling Github and Documentation and only supporting the backend and Carlos doing our Testing and Quality Assurance and being a support member for the frontend.

---

## Per-Member Contributions

### Matthew Bonnichsen - Repo & Documentation, Backend support
- Set up and organized the GitHub repository (structure, `README.md`, `docs/` folder).
- Authored and maintained the **Product Backlog** file so its history is tracked in the repo.
- Packaged the database as `schema.sql` for import and wrote the **MAMP/setup guide**.
- Produced the Windows build and published the GitHub Release.
- Provided backend support and general project coordination.

### Bryan Banuelos - Backend (PHP) & MySQL
- Designed the **MySQL schema** (users, products, cart, orders) and created the tables and relationships.
- Wrote the database connection layer (`db.php`).
- Implemented the **PHP REST endpoints**: register, login, add/get/delete addresses, get products (with search + sort), cart add/get/update/delete, discount validation, place order, order history, and modify-customer-info.
- Contributed to merging the frontend, backend, and database layers during integration.

### Aaron Garza - UI Design & Unity Frontend
- Designed the UI and built the **Unity customer-portal screens**: login + register, product catalog (search, sort, images), shopping cart (view/add/remove/quantity), address management, checkout (subtotal, tax, discount, delivery), and order history (sort by date/amount).

### Carlos Patiño - QA & Testing, Frontend support
- Co-authored the **user stories** and their natural-language test cases.
- Seeded the product catalog with grocery items + images and the discount codes.
- Built the **server request layer** (`UnityWebRequest` + image loader) and scene navigation / user session handling.
- Performed **end-to-end QA** of the full flow (register → browse → cart → checkout → order history).

---

## Balance Statement

Work was distributed evenly across the four system layers, with each member
owning one layer end to end and supporting integration. No single member's
scope was substantially larger than another's; the uneven raw commit counts
are an artifact of how frontend work was merged and also artifacts sent outside of github, not of unequal effort.
