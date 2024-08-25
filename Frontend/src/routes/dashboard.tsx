/* eslint-disable react/react-in-jsx-scope */
import { useQuery } from '@tanstack/react-query';
import { createFileRoute, Navigate, useRouter } from '@tanstack/react-router'
import { AppContext } from '../main';
import { useContext } from 'react';
import { getSpacesByUserId } from '../api/endpoints';
import { Space } from '../api/types';

export const Route = createFileRoute('/dashboard')({
  component: () => <>
    <Dashboard />
  </>
});

export default function Dashboard() {
  const context = useContext(AppContext);
  const { currentUser } = context;

  const router = useRouter();

  const spacesQuery = useQuery({
    queryKey: ["dashboard-spaces", currentUser!.guid],
    queryFn: (context) => getSpacesByUserId(context.queryKey[1]),
    enabled: currentUser !== undefined
  });

  if (!currentUser) {
    const errorMessage = `Context lacks currentUser`
    console.error(errorMessage);
    return <Navigate to="/login" />
  }

  const goToSpace = (space: Space) => {
    context.currentSpace = spacesQuery.data!.find(querySpace => querySpace.guid == space.guid)!;
    router.navigate({ to: "/space" })
  };

  if (!currentUser) {
    return <Navigate to="/login"></Navigate>
  }

  return (<>
    <h1>{`Hello ${currentUser.alias}!`}</h1>
    {spacesQuery.data && spacesQuery.data.map(space =>
      <button
        key={space.guid}
        onClick={() => goToSpace(space)}>
        {space.alias}
      </button>)}
  </>)
}