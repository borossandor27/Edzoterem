import { createContext, useContext, useState, type ReactNode } from "react";
import { bejelentkezes as bejelentkezesApi } from "../api/auth";
import { tokenMentese, tokenTorlese, tokenOlvasasa } from "../api/client";
import type { Szerepkor } from "../types";

const SESSION_META_KULCS = "edzoterem_session_meta";

interface SessionMeta {
  szerepkor: Szerepkor;
  nev: string;
  jelszoIdeiglenes: boolean;
}

interface AuthContextValue {
  bejelentkezve: boolean;
  szerepkor: Szerepkor | null;
  nev: string | null;
  jelszoIdeiglenes: boolean;
  bejelentkezes: (felhasznalonev: string, jelszo: string) => Promise<void>;
  kijelentkezes: () => void;
  jelszoCsereMegtortent: () => void;
}

function metaBetoltese(): SessionMeta | null {
  const nyers = localStorage.getItem(SESSION_META_KULCS);
  if (!nyers || !tokenOlvasasa()) {
    return null;
  }
  try {
    return JSON.parse(nyers) as SessionMeta;
  } catch {
    return null;
  }
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [meta, setMeta] = useState<SessionMeta | null>(metaBetoltese);

  async function bejelentkezes(felhasznalonev: string, jelszo: string) {
    const valasz = await bejelentkezesApi(felhasznalonev, jelszo);
    tokenMentese(valasz.token);
    const ujMeta: SessionMeta = {
      szerepkor: valasz.szerepkor,
      nev: valasz.nev,
      jelszoIdeiglenes: valasz.jelszoIdeiglenes,
    };
    localStorage.setItem(SESSION_META_KULCS, JSON.stringify(ujMeta));
    setMeta(ujMeta);
  }

  function kijelentkezes() {
    tokenTorlese();
    localStorage.removeItem(SESSION_META_KULCS);
    setMeta(null);
  }

  function jelszoCsereMegtortent() {
    if (!meta) return;
    const ujMeta = { ...meta, jelszoIdeiglenes: false };
    localStorage.setItem(SESSION_META_KULCS, JSON.stringify(ujMeta));
    setMeta(ujMeta);
  }

  const value: AuthContextValue = {
    bejelentkezve: meta !== null,
    szerepkor: meta?.szerepkor ?? null,
    nev: meta?.nev ?? null,
    jelszoIdeiglenes: meta?.jelszoIdeiglenes ?? false,
    bejelentkezes,
    kijelentkezes,
    jelszoCsereMegtortent,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth csak AuthProvider-en belül használható.");
  }
  return context;
}
