/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'
import { useContext } from 'react';
import { AppContext } from '../main';
import Conversation from '../components/Conversation';

export const Route = createFileRoute('/space')({
  component: () => <Space />
})

export default function Space() {
  const { currentUser, currentSpace } = useContext(AppContext);

  if (!currentSpace || !currentUser) {
    const errorMessage = `currentSpace or currentUser is undefined}`
    console.error(errorMessage);
    return <p>{errorMessage}</p>
  }

  return (
    <>
      <p>{`${currentSpace.alias} Space`}</p>
      <Conversation />
    </>
  );
}