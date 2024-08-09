import React from 'react'
import './App.css'
import { PrimitiveDataTable } from './components/dataTable'
import User from './types/user'



function App() {
  return (
    <>
      <PrimitiveDataTable<User> />
    </>
  )
}

export default App
