// A mysql2 pool `dateStrings: true` móddal "YYYY-MM-DD HH:MM:SS" alakban adja vissza a
// DATETIME oszlopokat; ISO-szerű "YYYY-MM-DDTHH:MM:SS" formára hozzuk, hogy a kliensek
// (amik korábban .NET DateTime JSON szerializációt kaptak) ugyanazt a formátumot lássák.
export function toIso(value) {
  if (value === null || value === undefined) {
    return null;
  }
  return value.replace(" ", "T");
}
