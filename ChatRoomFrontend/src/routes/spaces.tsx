/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/spaces')({
  validateSearch: (search) => {
    return { user: (search.user as string) };
  },
  component: () => <Spaces />
});

export default function Spaces() {
  const { user } = Route.useSearch();
  return <>{`Hello ${user}!`}</>
}