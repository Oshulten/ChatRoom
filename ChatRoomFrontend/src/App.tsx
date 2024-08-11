/* eslint-disable react/react-in-jsx-scope */
import { useEffect, useState } from 'react';
import './App.css'
import { ChatUserClass } from './types/chatUser';
import { ChatMessageClass } from './types/chatMessage';
import { ChatSpaceClass } from './types/chatSpace';
import { useImmer } from 'use-immer';
import { InteractiveDataRow } from './components/dataTable2';

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server
//Todo: object loses it's class identity after reassembly

// const baseUrl = "http://localhost:5055/api";

export default function App() {
  // const [user, setUser] = useState<ChatUserClass>(ChatUserClass.fromProperties("Mike", "Mike", "myPassowrd", new Date("2024-08-11T13:00:00.000+02:00"), false));
  // const [message, setMessage] = useState<ChatMessageClass>(ChatMessageClass.fromProperties("MikeMessage", "Mike", new Date("2024-08-11T13:00:00.000+02:00"), "Hello no-one!", "global"));
  // const [space, setSpace] = useState<ChatSpaceClass>(ChatSpaceClass.fromProperties("global", "Global", ["Mike"]));

  // const [users, setUsers] = useImmer<ChatUserClass[]>([]);
  // const [messages, setMessages] = useImmer<ChatMessageClass[]>([]);
  // const [spaces, setSpaces] = useImmer<ChatSpaceClass[]>([]);


  // useEffect(() => {
  //   const getUsers = async () => {
  //     try {
  //       const response = await fetch(`${baseUrl}/ChatUsers`);
  //       if (!response.ok) {
  //         throw new Error(`Response status: ${response.status}`);
  //       }
  //       const json = await response.json() as ChatUserClass[];
  //       setUsers(json);
  //     } catch (error) {
  //       console.error(`Something went wrong on the backend ${(error as Error).message}`);
  //     }
  //   }

  //   const getMessages = async () => {
  //     try {
  //       const response = await fetch(`${baseUrl}/ChatUsers`);
  //       if (!response.ok) {
  //         throw new Error(`Response status: ${response.status}`);
  //       }
  //       const json = await response.json() as ChatMessageClass[];
  //       setMessages(json);
  //     } catch (error) {
  //       console.error(`Something went wrong on the backend ${(error as Error).message}`);
  //     }
  //   }

  //   const getSpaces = async () => {
  //     try {
  //       const response = await fetch(`${baseUrl}/ChatUsers`);
  //       if (!response.ok) {
  //         throw new Error(`Response status: ${response.status}`);
  //       }
  //       const json = await response.json() as ChatSpaceClass[];
  //       setSpaces(json);
  //     } catch (error) {
  //       console.error(`Something went wrong on the backend ${(error as Error).message}`);
  //     }
  //   }

  //   getUsers();
  //   getSpaces();
  //   getMessages();
  // }, [])

  return (
    <>
      <InteractiveDataRow />
    </>
  )
}
