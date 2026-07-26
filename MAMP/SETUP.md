# Backend Setup MAMP + MySQL

The Online Grocery Store has two halves that must both run:

1. **The frontend** — the Unity build (`.exe`) or the Unity project you press Play on.
2. **The backend** — MAMP (Apache + MySQL + PHP) serving the `CustomerPortal_MAMP` folder.

The frontend won't load products, log in, or place orders unless the backend is running **first**. Set this up once; after that it's just "start MAMP, launch the app."

---

## Step 1 — Install MAMP

- Download **MAMP** (free) from <https://www.mamp.info/en/downloads/>.
- Windows installs to `C:\MAMP\`; macOS to `/Applications/MAMP/`.
- Use plain **MAMP** (not MAMP PRO).

## Step 2 — Set the port to 80

The app talks to **`http://localhost/CustomerPortal`** (port 80), so MAMP's web server must use port 80.
- MAMP → **Preferences → Ports** → **"Set Web & MySQL ports to 80 & 3306"** → OK.

## Step 3 — Copy the backend into htdocs and rename it

- Copy the repo's **`CustomerPortal_MAMP`** folder into MAMP's web root:
  - Windows: `C:\MAMP\htdocs\`
  - macOS: `/Applications/MAMP/htdocs/`
- **Rename the copied folder to `CustomerPortal`.**
  - Final path: `…/htdocs/CustomerPortal/` with all `.php` files, `schema.sql`, and `images/` inside.
  - This makes it reachable at `http://localhost/CustomerPortal`, which is exactly what the app calls.

## Step 4 — Start the servers

- MAMP → **Start Servers** → wait for **Apache** and **MySQL** lights to turn **green**.
- The WebStart page should open; if not, go to <http://localhost/>.

## Step 5 — Create the database (import `schema.sql`)

- Open phpMyAdmin: WebStart page → **Tools → phpMyAdmin**, or <http://localhost/phpMyAdmin/>.
- Click the **home icon** (top-left) so **no database is selected** — the script creates the database itself.
- **Import** tab → **Choose File** → select `CustomerPortal/schema.sql` → **Go**.
- Success = green bar and a **`CustomerPortal`** database with 7 tables. Click **products → Browse** to see 33 rows.
- One-time step; the data persists afterward.

## Step 6 — Confirm the DB login (already correct by default)

`CustomerPortal/db.php` connects with host `localhost`, user `root`, password `root`, database `CustomerPortal` — MAMP's defaults, so no change needed.
*(XAMPP only: its root password is blank, so set `$DbPassword = "";` in `db.php`.)*

## Step 7 — Smoke-test the backend

Open in a browser:
- <http://localhost/CustomerPortal/getProducts.php?search=> → JSON: `{"success":true,"message":"Products loaded.","products":[…]`
- <http://localhost/CustomerPortal/images/apples.jpg> → the apple photo loads.

If both work, the backend is live. Launch the game (see [DOWNLOAD.md](DOWNLOAD.md)) or press Play in Unity.

---

## Everyday use

1. MAMP → **Start Servers** (green lights).
2. Launch the `.exe` (or press Play in Unity).
3. **Stop Servers** when done.

---

## Test data

| Discount code | Effect |
|---|---|
| `SAVE10` | 10% off |
| `WELCOME15` | 15% off |
| `STUDENT20` | 20% off |
| `EXPIRED` | inactive — tests the "not valid" path |

Delivery fees: Pickup $0.00 · Standard $4.99 · Express $9.99. Tax: 8.25%.

---

## Troubleshooting

| Symptom | Fix |
|---|---|
| MAMP window blank/white, no lights | Quit MAMP fully (Task Manager → end `MAMP`, `httpd`, `mysqld`), relaunch **Run as administrator** (Windows). |
| Apache won't start / stays red | Port 80 in use (IIS, Skype). Free it, or the build can't connect. |
| App loads but no products / can't log in | Backend not running, or Apache not on port 80. Recheck Steps 2 & 4, re-run Step 7 URLs. |
| `Database connection failed.` | `schema.sql` not imported, or `db.php` creds don't match. Redo Steps 5–6. |
| Browser shows raw PHP code | You opened the file directly. Always use `http://localhost/CustomerPortal/…`. |
| 404 on test URLs | Folder isn't in `htdocs`, not renamed to `CustomerPortal`, or wrong port. Recheck Step 3. |
| Products load but images broken | `images/` didn't copy. Check `http://localhost/CustomerPortal/images/apples.jpg`. |

**Reset between test runs:** in phpMyAdmin, **Empty** `cart_items` / `orders` / `order_items`, or re-import `schema.sql` to wipe and reseed everything.
