# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

Regula R7 statea in starter in `Services/BookingService.cs` intr o bucla foreach si acum sta in `Domain/Bookings/Booking.cs` in metoda `IsActiveAndOverlaps`. Am mutat o acolo pentru ca tine strict de logica unei rezervari si locul ei este in entitatea din domeniu, nu intr un serviciu extern. Este scrisa ca Expression ca sa poata fi tradusa direct in SQL de catre EF Core fara sa duplicam conditia in mai multe locuri.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

Faptul ca sala este ocupata este un scenariu normal de business, nu o eroare neasteptata de sistem, deci nu merita costul unei exceptii sau fluxuri de control ascunse. Cu `Result`, se vede clar in semnatura metodei ca operatia poate esua si ce fel de eroare intoarce. Cand handlerul returneaza un `Error`, endpointul apeleaza `result.Error.ToProblem()` din `Common/ErrorExtensions.cs`, care mapeaza tipul erorii la codul HTTP corespunzator (Validation la 400, NotFound la 404, Conflict la 409) cu codul erorii in title si mesajul in detail.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Doar clasele din folderul `Infrastructure/` stiu de EF Core, adica `RoomHubDbContext`, clasele din `Configurations/` si implementarile din `Repositories/`. Straturile `Domain/` si `Features/` nu au nicio referinta la EF Core si lucreaza exclusiv prin interfetele `IRoomRepository`, `IBookingRepository` si `IUnitOfWork`, singura concesie fiind constructorul privat fara parametri din entitati. Castigam un domeniu usor de testat si posibilitatea de a schimba baza de date fara sa atingem logica, dar platim prin mai multe fisiere scrise si prin faptul ca unele optimizari specifice EF Core, cum ar fi `AsNoTracking`, sunt mai greu de expus curat prin interfete.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

CreateBookingEndpoint
-> CreateBookingCommand
-> CreateBookingHandler
-> IRoomRepository.GetByIdAsync               (404 RoomNotFound)
-> Booking.Create(room, ..., nowUtc)          (R2-R6, R10 -> 400)
-> IBookingRepository.HasActiveOverlapAsync   (R7 -> 409)
-> IBookingRepository.CountActiveForParticipantOnDayAsync (R9 -> 409)
-> IBookingRepository.Add
-> IUnitOfWork.SaveChangesAsync (RoomHubDbContext) -> SQLite roomhub.db
<- Result<CreateBookingResponse> -> 201 + Location  /  Error.ToProblem()

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

Am respins propunerea AI ului de a face el commit urile in git si am ales sa le fac eu manual dupa fiecare etapa, ca sa am control pe istoricul din repository si sa verific fiecare modificare inainte de push. De asemenea, i am cerut sa stearga fisierul cu raportul de analiza generat la inceput ca sa nu ramana fisiere inutile in repo si am impus sa nu puna comentarii in cod.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

As adauga teste unitare in xUnit pentru `Booking.Create` si `Booking.Cancel` folosind `FakeTimeProvider`, inclusiv pentru regulile R9, R10 si R11 (`IsActiveForParticipantOnDay`, `ReachesDailyLimit` si `MaxBookingDuration`). Ca riscuri ramase in proiect, exista o problema de concurenta la R7 si R9 unde doua cereri simultane pot trece amandoua de verificare inainte de salvare, `EnsureCreated` nu suporta migratii daca se schimba schema bazei, iar la R9 comparatia pentru `bookedBy` este case sensitive.