import React from 'react'
import './App.css'
import PrimitiveDataTable from './components/dataTable'
import ChatUser from './types/chatUser'
// import ChatMessage from './types/chatMessage'
import ChatSpace from './types/chatSpace'

const baseUrl = "http://localhost:5055/api";

function App() {
  return (
    <>
      {/* <PrimitiveDataTable<ChatUser> endpoint={`${baseUrl}/ChatUsers`} showId={true} /> */}
      <PrimitiveDataTable<ChatSpace> endpoint={`${baseUrl}/ChatSpaces`} showId={true} />
      {/* <PrimitiveDataTable<ChatMessage> endpoint={`${baseUrl}/ChatMessages`} showId={true} /> */}
    </>
  )
}

export default App
