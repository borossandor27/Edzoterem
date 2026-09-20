const API_BASE_URL = import.meta.env.VITE_API_BASE_URL as string;
const TOKEN_KULCS = "edzoterem_token";

export class ApiHiba extends Error {
  status: number;

  constructor(message: string, status: number) {
    super(message);
    this.status = status;
  }
}

export function tokenMentese(token: string) {
  localStorage.setItem(TOKEN_KULCS, token);
}

export function tokenTorlese() {
  localStorage.removeItem(TOKEN_KULCS);
}

export function tokenOlvasasa(): string | null {
  return localStorage.getItem(TOKEN_KULCS);
}

async function kerelem<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = tokenOlvasasa();
  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...(options.headers as Record<string, string> | undefined),
  };

  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(`${API_BASE_URL}${path}`, { ...options, headers });

  if (!response.ok) {
    let uzenet = `Hiba történt (${response.status}).`;
    try {
      const body = await response.json();
      if (body?.uzenet) {
        uzenet = body.uzenet;
      }
    } catch {
      // a válasz nem JSON - marad az alap üzenet
    }
    throw new ApiHiba(uzenet, response.status);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

export const apiClient = {
  get: <T>(path: string) => kerelem<T>(path, { method: "GET" }),
  post: <T>(path: string, body: unknown) =>
    kerelem<T>(path, { method: "POST", body: JSON.stringify(body) }),
  put: <T>(path: string, body?: unknown) =>
    kerelem<T>(path, { method: "PUT", body: body ? JSON.stringify(body) : undefined }),
  del: <T>(path: string) => kerelem<T>(path, { method: "DELETE" }),
};
