/* eslint-disable react/react-in-jsx-scope */
import './App.css'
import AdminLayout from './components/adminLayout';

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server

export default function App() {
  return (
    <>
      <AdminLayout />
    </>
  )
}
