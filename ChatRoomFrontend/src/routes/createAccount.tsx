/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute, useRouter } from '@tanstack/react-router'
import AuthenticateForm from '../components/authenticateForm';
import { createUser } from '../api/endpoints';
import { useState } from 'react';
import { AuthenticationRequest } from '../api/types';

export const Route = createFileRoute('/createAccount')({
  component: () => <CreateAccount />
})

export const baseUrl = "http://localhost:5055/api";

function CreateAccount() {
  const [formMessage, setFormMessage] = useState<string | undefined>(undefined);
  const router = useRouter();

  const handleSubmit = async (fields: AuthenticationRequest) => {
    try {
      const createdUser = await createUser(fields);
      router.navigate({ to: "/spaces", search: { user: createdUser.alias } })
    }
    catch (error) {
      setFormMessage((error as Error).message)
    }
  }

  return (
    <div className="p-4 rounded-lg bg-slate-800 border-2" >
      <h2>Create Account</h2>
      <AuthenticateForm
        submitLabel='Create Account'
        onSuccess={(fields) => handleSubmit(fields)}
        errorMessage={formMessage} />
      <button onClick={() => router.navigate({ to: "/login" })}>Already a user?</button>
    </div>)
}
