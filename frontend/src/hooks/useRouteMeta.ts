import { useMatches } from "react-router";

export function useRouteMeta<T>() {
  const matches = useMatches();
  return matches[matches.length - 1]?.handle as T;
}
