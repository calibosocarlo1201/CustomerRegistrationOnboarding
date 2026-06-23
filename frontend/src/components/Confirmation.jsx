export default function Confirmation({ customer, onRegisterAnother }) {
  return (
    <div className="confirmation">
      <div className="confirmation-icon">✓</div>
      <h1>Registration Complete</h1>
      <p>The customer has been successfully registered.</p>

      <div className="confirmation-details">
        <p><strong>Name:</strong> {customer.firstName} {customer.lastName}</p>
        <p><strong>Email:</strong> {customer.email}</p>
        <p><strong>Phone:</strong> {customer.phoneNumber}</p>
        <p><strong>Customer ID:</strong> {customer.id}</p>
        <p><strong>Date Created:</strong> {new Date(customer.dateCreated).toLocaleString()}</p>
      </div>

      <button type="button" className="btn-primary" onClick={onRegisterAnother}>
        Register Another Customer
      </button>
    </div>
  );
}
