# User Stories

**Customers**
- As a guest, I want to register for an account so that I can save my information and place orders.
- As a registered customer, I want to log in to my account so that I can access my saved information and order history.
- As a logged-in customer, I want to add an address to my account so that I can use it for shipping and billing.
- As a logged-in customer, I want to modify my own customer information so that my account details stay accurate and up to date.

**Shopping Cart**
- As a customer, I want to see all items currently in my cart so that I can review what I plan to purchase.
- As a customer, I want to add or subtract items in my cart, including adjusting the quantity of each item, so that I can control exactly what I'm buying.

**Checkout**
- As a customer, I want the system to calculate the total cost of items in my cart before tax so that I know my subtotal.
- As a customer, I want to clearly see the calculated tax (at a rate of 8.25%) so that I understand how much tax I'm being charged.
- As a customer, I want to see the total cost including tax so that I know the final amount I will be charged.
- As a customer, I want to enter a discount code at checkout so that I can apply eligible savings to my order.
- As a customer, I want to choose from more than one type of delivery option so that I can pick the shipping speed and cost that works best for me.
- As a customer, I want to see a summary of my order and confirm before placing it so that I can verify everything is correct before completing my purchase.

**Browse Items**
- As a customer, I want to use a search bar so that I can quickly find products I'm interested in.
- As a customer, I want to search for items by item name or description so that I can find products even if I don't know the exact name.
- As a customer, I want to sort search results using filters such as price and availability so that I can find items that match my preferences.
- As a customer, I want to see each item's picture, name, and price in the listing so that I can quickly evaluate products without opening each one.

**Order History**
- As a customer, I want to sort my order history by order date so that I can easily find recent or past orders.
- As a customer, I want to sort my order history by order size in USD amount so that I can identify my largest or smallest purchases.

---

# Testable User Stories

Acceptance criteria are written in Given/When/Then format.

## Table of Contents

- [Customer](#customer)
- [Shopping Cart](#shopping-cart)
- [Check Out](#check-out)
- [Browse Items](#browse-items)
- [Order History](#order-history)

---

## Customer

### 1. Account Registration
**As a** guest, **I want to** register for an account **so that** I can save my information and place orders.

**Acceptance Criteria:**
- Given a guest is on the registration page, when they submit a valid email, password, and required fields, then a new account is created and they are logged in.
- Given a guest enters an email that is already registered, when they submit the form, then an error message is shown and no duplicate account is created.
- Given a guest enters a password that does not meet minimum requirements, when they submit the form, then a validation error is displayed and the account is not created.

### 2. Log In
**As a** registered customer, **I want to** log in to my account **so that** I can access my saved information and order history.

**Acceptance Criteria:**
- Given a registered customer enters a correct email and password, when they submit the login form, then they are authenticated and redirected to their account/home page.
- Given a customer enters an incorrect password, when they submit the login form, then an error message is shown and they remain logged out.
- Given a customer fails to log in 5 times in a row, when they attempt a 6th time, then the account is temporarily locked or a CAPTCHA is required.

### 3. Add an Address
**As a** logged-in customer, **I want to** add an address to my account **so that** I can use it for shipping and billing.

**Acceptance Criteria:**
- Given a logged-in customer is on the address page, when they submit a complete, valid address, then the address is saved and appears in their address list.
- Given a customer submits an address with missing required fields (e.g., street, city, zip), when they submit the form, then validation errors are shown and the address is not saved.
- Given a customer has at least one saved address, when they view their address list, then all saved addresses are displayed.

### 4. Modify Customer Information
**As a** logged-in customer, **I want to** modify my own customer information **so that** my account details stay accurate and up to date.

**Acceptance Criteria:**
- Given a logged-in customer edits an existing field (name, email, phone, etc.) with valid data, when they save the change, then the updated information is reflected immediately on the account page.
- Given a customer attempts to change their email to one already in use by another account, when they save, then an error is shown and the change is rejected.
- Given a customer leaves a required field blank, when they attempt to save, then a validation error is shown and no changes are persisted.

---

## Shopping Cart

### 5. View Cart Items
**As a** customer, **I want to** see all items currently in my cart **so that** I can review what I plan to purchase.

**Acceptance Criteria:**
- Given a customer has 1 or more items in their cart, when they open the cart view, then each item's name, image, unit price, quantity, and line-item subtotal are displayed.
- Given a customer's cart is empty, when they open the cart view, then a clear "cart is empty" message is displayed instead of an item list.

### 6. Add/Subtract Cart Items
**As a** customer, **I want to** add or subtract items in my cart, including adjusting the quantity of each item, **so that** I can control exactly what I'm buying.

**Acceptance Criteria:**
- Given a customer is viewing an item, when they click "Add to Cart," then the item appears in the cart with a quantity of 1 (or increments by 1 if already present).
- Given an item is in the cart, when the customer increases or decreases the quantity, then the cart updates the line-item subtotal and cart total accordingly.
- Given an item's quantity is reduced to 0, when the update is applied, then the item is removed from the cart entirely.
- Given a customer attempts to set a quantity higher than available stock, when they submit the change, then an error message is shown and the quantity is capped at available stock.

---

## Check Out

### 7. Pre-Tax Subtotal
**As a** customer, **I want** the system to calculate the total cost of items in my cart before tax **so that** I know my subtotal.

**Acceptance Criteria:**
- Given a cart with one or more items, when the customer reaches checkout, then the pre-tax subtotal equals the sum of (unit price × quantity) for all items in the cart.
- Given the cart contents change at checkout, when the update completes, then the pre-tax subtotal recalculates automatically.

### 8. Display Calculated Tax
**As a** customer, **I want to** clearly see the calculated tax (at a rate of 8.25%) **so that** I understand how much tax I'm being charged.

**Acceptance Criteria:**
- Given a pre-tax subtotal, when checkout totals are displayed, then the tax line shows subtotal × 0.0825, rounded to the nearest cent.
- Given the subtotal changes (e.g., item added/removed, discount applied), when totals are recalculated, then the displayed tax amount updates to match the new subtotal.

### 9. Total Cost Including Tax
**As a** customer, **I want to** see the total cost including tax **so that** I know the final amount I will be charged.

**Acceptance Criteria:**
- Given a pre-tax subtotal and calculated tax, when checkout totals are displayed, then the final total equals subtotal + tax (minus any applied discount, if applicable) and is clearly labeled.
- Given any change to the cart, discount, or delivery option, when totals refresh, then the final total updates to reflect the change before the order can be placed.

### 10. Discount Codes
**As a** customer, **I want to** enter a discount code at checkout **so that** I can apply eligible savings to my order.

**Acceptance Criteria:**
- Given a customer enters a valid, active discount code, when they apply it, then the discount is reflected in the order totals before tax is recalculated.
- Given a customer enters an invalid, expired, or already-used code, when they attempt to apply it, then an error message is shown and no discount is applied.
- Given a discount has been applied, when the customer removes it, then the order totals revert to their pre-discount values.

### 11. Multiple Delivery Options
**As a** customer, **I want to** choose from more than one type of delivery option **so that** I can pick the shipping speed and cost that works best for me.

**Acceptance Criteria:**
- Given a customer is at checkout, when they view delivery options, then at least 2 delivery methods are displayed, each with its own cost and estimated delivery time.
- Given a customer selects a delivery option, when the selection is made, then the order total updates to include the selected delivery cost.
- Given no delivery option has been selected, when the customer attempts to place the order, then they are prompted to select one before proceeding.

### 12. Order Summary & Placement
**As a** customer, **I want to** see a summary of my order and confirm before placing it **so that** I can verify everything is correct before completing my purchase.

**Acceptance Criteria:**
- Given a customer reaches the final checkout step, when the order summary loads, then it displays all items, quantities, subtotal, discount (if any), tax, delivery method/cost, and final total.
- Given the order summary is accurate, when the customer clicks "Place Order," then the order is submitted and a confirmation is displayed.
- Given a customer wants to make a change, when they navigate back from the summary screen, then the order has not yet been placed.

---

## Browse Items

### 13. Search Bar
**As a** customer, **I want to** use a search bar **so that** I can quickly find products I'm interested in.

**Acceptance Criteria:**
- Given a customer types a search term and submits it, when results load, then only items matching the search term are displayed.
- Given a search term matches no items, when results load, then a "no results found" message is displayed.

### 14. Search by Item/Description
**As a** customer, **I want to** search for items by item name or description **so that** I can find products even if I don't know the exact name.

**Acceptance Criteria:**
- Given a search term matches text in an item's name, when results load, then that item is included in the results.
- Given a search term matches text only in an item's description (not its name), when results load, then that item is still included in the results.

### 15. Sort Filters
**As a** customer, **I want to** sort search results using filters such as price and availability **so that** I can find items that match my preferences.

**Acceptance Criteria:**
- Given a list of search/browse results, when the customer selects "Price: Low to High" or "Price: High to Low," then results reorder accordingly.
- Given a list of search/browse results, when the customer filters by "In Stock" availability, then only in-stock items are displayed.
- Given a filter is applied, when the customer clears it, then the full, unfiltered result set is restored.

### 16. Item Picture, Name, and Price
**As a** customer, **I want to** see each item's picture, name, and price in the listing **so that** I can quickly evaluate products without opening each one.

**Acceptance Criteria:**
- Given any browse or search results page, when items are displayed, then each item shows its picture, name, and price.
- Given an item has no picture available, when it is displayed, then a placeholder image is shown instead of a broken or blank image.

---

## Order History

### 17. Sort by Order Date
**As a** customer, **I want to** sort my order history by order date **so that** I can easily find recent or past orders.

**Acceptance Criteria:**
- Given a customer has 2 or more past orders, when they select "Sort by Date," then orders are displayed in chronological order (with a clear newest-first or oldest-first toggle).
- Given the sort order changes, when the page updates, then the order list re-renders in the newly selected order without page reload errors.

### 18. Sort by Order Size (USD)
**As a** customer, **I want to** sort my order history by order size in USD amount **so that** I can identify my largest or smallest purchases.

**Acceptance Criteria:**
- Given a customer has 2 or more past orders, when they select "Sort by Order Total," then orders are displayed in ascending or descending order by total USD amount.
- Given two orders have the same total amount, when sorted by amount, then a secondary, consistent tiebreaker (such as order date) determines their relative order.
