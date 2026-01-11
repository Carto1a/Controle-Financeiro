import { api } from "../api/axios";

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

  porPessoa: async (signal: AbortSignal | undefined = undefined): Promise<ResumoFinanceiro[]> =>
    (await api.get('/resumos/financeiro/pessoas', { signal: signal })).data,

  porCategoria: async (signal: AbortSignal | undefined = undefined): Promise<ResumoFinanceiro[]> =>
    (await api.get('/resumos/financeiro/categorias', { signal: signal })).data,
};

