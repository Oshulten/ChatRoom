/* eslint-disable react/react-in-jsx-scope */
import { useQuery } from '@tanstack/react-query';
import { createFileRoute, Navigate, useSearch } from '@tanstack/react-router'
import { GlobalContext } from '../main';
import { useContext } from 'react';
import { getSpacesByUserId } from '../api/endpoints';

export const Route = createFileRoute('/spaces')({
  validateSearch: (search) => {
    return { user: (search.user as string) };
  },
  component: () => <Spaces />
});

export default function Spaces() {
  const context = useContext(GlobalContext);
  const userName = useSearch({ from: '/spaces' }).user;
  const signedInUser = context.signedInAs;
  const spacesQuery = useQuery({
    queryKey: ["spaces", signedInUser?.id],
    queryFn: (context) => getSpacesByUserId(context.queryKey[1]),
    enabled: signedInUser !== undefined
  });

  if (signedInUser && userName != signedInUser.alias) {
    return <Navigate to="/login"></Navigate>
  }

  if (signedInUser) {
    return (<>
      <h1>{`Hello ${signedInUser.alias}!`}</h1>
      {spacesQuery.data && spacesQuery.data.map(space => <p key={space.id}>{space.alias}</p>)}
    </>)
  }
  return <Navigate to="/login" from="/spaces" />
}