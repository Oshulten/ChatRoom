import React from 'react'
import './App.css'
import PrimitiveDataTable from './components/dataTable'
import User from './types/user'

function App() {
  return (
    <>
      <PrimitiveDataTable<User> endpoint="http://localhost:5055/api/Users" showId={true} />
    </>
  )
}

export default App
