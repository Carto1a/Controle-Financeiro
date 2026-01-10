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
  totalFinanceiro: async (): Promise<ResumoFinanceiroTotal> =>
    (await api.get('/resumos/financeiro/total')).data,
  porPessoa: async (): Promise<ResumoFinanceiro[]> =>
    (await api.get('/resumos/financeiro/pessoas')).data,
  porCategoria: async (): Promise<ResumoFinanceiro[]> =>
    (await api.get('/resumos/financeiro/categorias')).data,
};

