// A mysql2 a DECIMAL oszlopokat stringként adja vissza (a pontosság megőrzése végett),
// a kliensek viszont eddig .NET decimal -> JSON number szerializációt kaptak.
export function toNumberOrNull(value) {
  return value === null || value === undefined ? null : Number(value);
}
