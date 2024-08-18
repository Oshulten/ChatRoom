/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute, useRouter } from '@tanstack/react-router'
import AuthenticateForm from '../components/authenticateForm';
import { useState } from 'react';
import { AuthenticationRequest } from '../api/types';
import { authenticateUser } from '../api/endpoints';

export const baseUrl = "http://localhost:5055/api";

export const Route = createFileRoute('/login')({
  component: () => <Login />
})

function Login() {
  const [formMessage, setFormMessage] = useState<string | undefined>(undefined);
  const router = useRouter();

  const handleSubmit = async (fields: AuthenticationRequest) => {
    try {
      const existingUser = await authenticateUser(fields);
      router.navigate({ to: "/spaces", search: { user: existingUser.alias } })
    }
    catch (error) {
      setFormMessage((error as Error).message)
    }
  }

  return (
    <div className="p-4 rounded-lg bg-slate-800 border-2" >
      <h2>Welcome!</h2>
      <AuthenticateForm
        submitLabel='Login'
        onSuccess={(fields) => handleSubmit(fields)}
        errorMessage={formMessage} />
      <button onClick={() => router.navigate({ to: "/createAccount" })}>New here?</button>
    </div>)
}
