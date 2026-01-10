import axios from "axios";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 10000,
});

export interface Paginated<T> {
  page: number;
  pageSize: number;
  totalPages: number;
  total: number;
  items: T[];
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
