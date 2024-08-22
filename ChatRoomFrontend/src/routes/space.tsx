/* eslint-disable react/react-in-jsx-scope */
import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { createFileRoute, useSearch } from '@tanstack/react-router'
import { getLastMessagesInSpace, getUserByUserId } from '../api/endpoints';
import { useContext } from 'react';
import { GlobalContext } from '../main';
import Conversation from '../components/Conversation';

export const Route = createFileRoute('/space')({
  validateSearch: (search) => {
    return {
      spaceAlias: search.spaceAlias as string
    }
  },
  component: () => <Space />
})

export default function Space() {
  const context = useContext(GlobalContext);
  const { spaceAlias } = useSearch({ from: "/space" });

  if (!context.currentSpace || !context.currentUser) {
    const errorMessage = `Context lacks currentSpace or currentUser: ${JSON.stringify(context)}`
    console.error(errorMessage);
    return <p>{errorMessage}</p>
  }

  if (context.currentSpace.alias != spaceAlias) {
    const errorMessage = `Query space ${spaceAlias} doesn't match current space ${JSON.stringify(context.currentSpace)}`
    console.log(errorMessage);
    return <p>{errorMessage}</p>
  }

  return (
    <>
      <p>{`${spaceAlias} Space`}</p>
      {sortedMessages && usersQuery.data && <Conversation messages={sortedMessages} users={usersQuery.data!} />}
    </>
  );
}