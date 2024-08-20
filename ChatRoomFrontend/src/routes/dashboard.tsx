/* eslint-disable react/react-in-jsx-scope */
import { useQuery } from '@tanstack/react-query';
import { createFileRoute, Navigate, useRouter, useSearch } from '@tanstack/react-router'
import { GlobalContext } from '../main';
import { useContext } from 'react';
import { getSpacesByUserId } from '../api/endpoints';
import { Space } from '../api/types';

export const Route = createFileRoute('/dashboard')({
  validateSearch: (search) => {
    return { user: (search.user as string) };
  },
  component: () => <Spaces />
});

export default function Spaces() {
  const context = useContext(GlobalContext);
  const userName = useSearch({ from: '/dashboard' }).user;
  const router = useRouter();
  const signedInUser = context.currentUser;
  const spacesQuery = useQuery({
    queryKey: ["dashboard-spaces", signedInUser?.id],
    queryFn: (context) => getSpacesByUserId(context.queryKey[1]),
    enabled: signedInUser !== undefined
  });

  console.log(`Context: ${JSON.stringify(context)}`);

  const goToSpace = (space: Space) => {
    const currentSpace = spacesQuery.data!.find(querySpace => querySpace.id == space.id)!;
    context.currentSpace = currentSpace;
    router.navigate({ to: "/space", search: { spaceAlias: currentSpace.alias } })
  };

  if (signedInUser && userName != signedInUser.alias) {
    return <Navigate to="/login"></Navigate>
  }

  if (signedInUser) {
    return (<>
      <h1>{`Hello ${signedInUser.alias}!`}</h1>
      {spacesQuery.data && spacesQuery.data.map(space =>
        <button
          key={space.id}
          onClick={() => goToSpace(space)}>
          {space.alias}
        </button>)}
    </>)
  }
  return <Navigate to="/login" from="/dashboard" />
}