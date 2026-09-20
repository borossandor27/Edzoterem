import { randomBytes } from "node:crypto";

const KARAKTEREK = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";

export function jelszoGeneralas(hossz = 10) {
  const buffer = randomBytes(hossz);
  let eredmeny = "";
  for (let i = 0; i < hossz; i++) {
    eredmeny += KARAKTEREK[buffer[i] % KARAKTEREK.length];
  }
  return eredmeny;
}
