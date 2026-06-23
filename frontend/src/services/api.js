const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5255/api';

export async function createCustomer(customerData) {
  const response = await fetch(`${API_BASE_URL}/customers`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(customerData),
  });

  const data = await response.json().catch(() => ({}));

  if (!response.ok) {
    const message = data.errors
      ? Object.values(data.errors).flat().join(' ')
      : data.title || 'Failed to register customer.';
    throw new Error(message);
  }

  return data;
}

export async function getCustomers() {
  const response = await fetch(`${API_BASE_URL}/customers`);
  if (!response.ok) {
    throw new Error('Failed to load customers.');
  }
  return response.json();
}
