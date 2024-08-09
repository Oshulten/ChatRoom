import React from 'react'
import './App.css'
import PrimitiveDataTable from './components/dataTable'
import ChatUser from './types/chatUser'

function App() {
  return (
    <>
      <PrimitiveDataTable<ChatUser> endpoint="http://localhost:5055/api/ChatUsers" showId={true} />
    </>
  )
}

export default App
