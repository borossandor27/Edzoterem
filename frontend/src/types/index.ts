export type Szerepkor = "Tulajdonos" | "Recepcios" | "Edzo" | "Tag";

export interface LoginResponse {
  token: string;
  szerepkor: Szerepkor;
  nev: string;
  jelszoIdeiglenes: boolean;
}

export interface TagResponse {
  id: number;
  vezeteknev: string;
  keresztnev: string;
  szuletesiDatum: string;
  cim: string | null;
  telefon: string | null;
  email: string | null;
  regisztracioDatuma: string;
  vhKapcsolattartoNev: string | null;
  vhKapcsolattartoTelefon: string | null;
  nem: string;
  megjegyzes: string | null;
  aktiv: boolean;
}

export interface BerletResponse {
  id: number;
  tagId: number;
  tagNev: string;
  berletTipusNev: string;
  kezdoDatum: string;
  lejaratiDatum: string;
  arFizetve: number;
  hatralevoAlkalmak: number | null;
  statusz: string;
  lejaro: boolean;
}

export interface CsoportosJelentkezesResponse {
  id: number;
  tagId: number;
  tagNev: string;
  jelenVolt: boolean | null;
}

export interface CsoportosFoglalkozasResponse {
  id: number;
  megnevezes: string;
  edzoId: number;
  edzoNev: string;
  idopont: string;
  helyszin: string;
  maxLetszam: number;
  statusz: string;
  elutasitasIndoka: string | null;
  jelentkezesek: CsoportosJelentkezesResponse[];
}

export interface EgyeniFoglalkozasResponse {
  id: number;
  szolgaltatasNev: string;
  dolgozoId: number;
  dolgozoNev: string;
  tagId: number;
  tagNev: string;
  idopont: string;
  ar: number;
  allapot: string;
  megjegyzes: string | null;
}
