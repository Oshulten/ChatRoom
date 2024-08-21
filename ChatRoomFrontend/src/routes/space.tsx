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

  const messagesQuery = useQuery({
    queryKey: ["messages", spaceAlias],
    queryFn: () => getLastMessagesInSpace(context.currentSpace!.id, new Date(), 5),
    enabled: context.currentSpace != undefined && context.currentUser != undefined,
    placeholderData: keepPreviousData,
  });

  const usersQuery = useQuery({
    queryKey: ["usersInSpace"],
    queryFn: async () => {
      const distinctUserIds = [...new Set(messagesQuery.data?.messages.map(message => message.userId))];
      const userPromises = distinctUserIds.map(id => getUserByUserId(id));
      const users = await Promise.all(userPromises);
      return users;
    },
    enabled: messagesQuery.isSuccess
  })


  if (!context.currentSpace) {
    console.log(`No space is set to current`);
  } else {
    if (context.currentSpace.alias != spaceAlias) console.log(`Query space ${spaceAlias} doesn't match current space ${JSON.stringify(context.currentSpace)}`);
  }


  const sortedMessages = messagesQuery.data
    ? messagesQuery.data.messages.sort((a, b) => {
      return (new Date(a.postedAt)) > (new Date(b.postedAt)) ? 1 : -1
    })
    : undefined;

  return (
    <>
      <p>{`${spaceAlias} Space`}</p>
      {sortedMessages && usersQuery.data && <Conversation messages={sortedMessages} users={usersQuery.data!} />}
    </>
  );
}