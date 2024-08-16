/* eslint-disable react/react-in-jsx-scope */
import { useQuery } from '@tanstack/react-query';
import { createFileRoute, useRouter } from '@tanstack/react-router'
import { createAccount } from '../api/createAccount';
import AuthenticateForm, { AuthenticationFields } from '../components/authenticateForm';
import { useState } from 'react';

export const Route = createFileRoute('/createAccount')({
  component: () => <CreateAccount />
})

export const baseUrl = "http://localhost:5055/api";

function CreateAccount() {
  const [loginField, setLoginField] = useState<AuthenticationFields | null>(null);
  const router = useRouter();

  const createAccountQuery = useQuery({
    queryKey: ["createAccount"],
    queryFn: () => createAccount(baseUrl, loginField!),
    enabled: loginField !== null
  })

  if (createAccountQuery.isSuccess) {
    if (createAccountQuery.data != null) {
      console.log(`Created account for ${createAccountQuery.data.alias}`)
      //navigate to space page
    } else {
      console.log(`Failed to create account`);
      //stay on page, give error message
    }
  }

  return (
    <div className="p-4 rounded-lg bg-slate-800 border-2" >
      <h2>Create Account</h2>
      <AuthenticateForm
        submitLabel='Create Account'
        onSuccess={(fields) => setLoginField(fields)} />
      <button onClick={() => router.navigate({ to: "/login" })}>Already a user?</button>
    </div>)
}
