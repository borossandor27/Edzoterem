function createEnum(names) {
  const toDb = {};
  const fromDb = {};
  names.forEach((name, index) => {
    toDb[name] = index;
    fromDb[index] = name;
  });
  return {
    toDb: (name) => {
      if (!(name in toDb)) {
        throw new Error(`Ismeretlen enum érték: ${name}`);
      }
      return toDb[name];
    },
    fromDb: (value) => fromDb[value],
  };
}

export const NEM = createEnum(["Ferfi", "No", "Egyeb"]);
export const SZEREPKOR = createEnum(["Tulajdonos", "Recepcios", "Edzo", "Tag"]);
export const HET_NAPJA = createEnum([
  "Hetfo",
  "Kedd",
  "Szerda",
  "Csutortok",
  "Pentek",
  "Szombat",
  "Vasarnap",
]);
export const BERLET_KATEGORIA = createEnum(["Napi", "Havi", "Feleves", "Eves", "Alkalmas"]);
export const BERLET_STATUSZ = createEnum(["Aktiv", "Lejart", "Felfuggesztve"]);
export const CSOPORTOS_STATUSZ = createEnum([
  "JovahagyasraVar",
  "Jovahagyva",
  "Elutasitva",
  "Lezarva",
]);
export const EGYENI_ALLAPOT = createEnum(["Lefoglalva", "Teljesitve", "Lemondva"]);
