/* eslint-disable react/react-in-jsx-scope */
import './App.css'
import AuthenticateForm from './components/authenticateForm'

export default function App() {
  return (
    <>
      <AuthenticateForm submitLabel='Login' onSuccess={(fields) => console.log(fields)} />
      <AuthenticateForm submitLabel='Create Account' onSuccess={(fields) => console.log(fields)} />
    </>)
}
