import React from 'react'
import './App.css'
import PrimitiveDataTable from './components/dataTable'
import ChatUser from './types/chatUser'
import ChatMessage from './types/chatMessage'
import ChatSpace from './types/chatSpace'

const baseUrl = "http://localhost:5055/api";

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server

function App() {
  return (
    <>
      {/* <PrimitiveDataTable<ChatUser> key="1" endpoint={`${baseUrl}/ChatUsers`} showId={true} /> */}
      <PrimitiveDataTable<ChatSpace> key="2" endpoint={`${baseUrl}/ChatSpaces`} showId={true} />
      {/* <PrimitiveDataTable<ChatMessage> key="3" endpoint={`${baseUrl}/ChatMessages`} showId={true} /> */}
    </>
  )
}

export default App
