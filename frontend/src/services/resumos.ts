import { api, type Paginated, type PaginatedQuery } from "../api/axios";

export interface ResumoFinanceiroTotal {
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface ResumoFinanceiro extends ResumoFinanceiroTotal {
  id: string;
  nome: string;
}

export const resumosService = {
  totalFinanceiro: async (signal: AbortSignal | undefined = undefined): Promise<ResumoFinanceiroTotal> =>
    (await api.get('/resumos/financeiro/total', { signal: signal })).data,

  porPessoa: async (query: PaginatedQuery, signal: AbortSignal | undefined = undefined): Promise<Paginated<ResumoFinanceiro>> =>
    (await api.get('/resumos/financeiro/pessoas', { params: query, signal: signal })).data,

  porCategoria: async (query: PaginatedQuery, signal: AbortSignal | undefined = undefined): Promise<Paginated<ResumoFinanceiro>> =>
    (await api.get('/resumos/financeiro/categorias', { params: query, signal: signal })).data,
};

