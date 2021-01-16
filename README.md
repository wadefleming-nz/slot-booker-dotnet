# Slot Booker
Automated bot to secure Managed Isolation slots with ease

## Background
New Zealand has implemented a strict Managed Isolation process to address COVID transmission, whereby incoming passengers must stay in government managed hotels for a 14 day period.

These slots must be secured before booking of flights. They are released 3 months in advance, and are generally booked out for the next 2 months. 

Slots in this 2 month period are sometimes available due to cancellations but are booked again very quickly(usually within minutes or less).

The general approach to secure these slots is to log in and manually refresh the availability calendar page at intervals as low as possible (typically 5 seconds) over an extended amount of time (typically several days).

When a slot is available it must be taken very quickly given other competition.

This app/bot automates the entire process (aside from solving the initial reCAPTCHA one time only).

## Usage

1. User starts App
2. User enters email and password for MIQ website
3. User clicks desired dates in calendar, in order of preference
4. User initiates search
5. Automated browser is opened at the MIQ login page with credentials prefilled
6. User manually solves reCAPTCHA then logs in
7. App takes over browser control and navigates through the site to the Availability calendar
8. App navigates to correct month(s) and checks for desired days
9. App refreshes page and checks calendar again
10. If desired day is available, it is booked
11. Browser closes and App reports success

## Technologies
- C# .NET
- Winforms
- Selenium WebDriver

