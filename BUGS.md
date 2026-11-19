# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |

|R7|Services/BookingService.cs, linia 28|Citind codul si testand cu curl|Am inlocuit semnul <= cu < in Booking.IsActiveAndOverlaps ca rezervarile consecutive care doar se ating sa nu dea 409|
|R7 / R8|Services/BookingService.cs, linia 26|Citind codul|Am adaugat filtrarea dupa statusul Active in aceeasi expresie ca o rezervare anulata sa nu mai blocheze intervalul|
|R8|Services/BookingService.cs, liniile 56–60|Testand cu curl|Am modificat Booking.Cancel sa intoarca eroarea AlreadyCancelled cu status 409 Conflict in loc de 204 la a doua anulare|
|R5|Services/BookingService.cs, metoda Create|Citind codul|Am adaugat verificarea ca startUtc sa nu fie in trecut fata de nowUtc obtinut din TimeProvider si sa intoarca 400|
|R6|Services/BookingService.cs, metoda Create|Citind codul|Am adaugat validarea pentru durata maxima de 4 ore direct in Booking.Create|
|Contract API|Middleware/ExceptionMiddleware.cs|Testand cu curl|Am inlocuit raspunsul text/plain de 400 la JSON invalid configurand AddProblemDetails si UseExceptionHandler in Program.cs|