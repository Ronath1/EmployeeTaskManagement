import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/authApi";
import { saveAuth } from "../utils/authStorage";

function LoginPage() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("admin@example.com");
  const [password, setPassword] = useState("Admin@12345");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setLoading(true);
      setError("");

      const data = await login(email, password);

      saveAuth(data.token, {
        email: data.email,
        fullName: data.fullName,
        role: data.role,
        expiresAt: data.expiresAt,
      });

      navigate("/dashboard");
    } catch (err) {
      setError("Invalid email or password.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="auth-page">
      <form className="auth-card" onSubmit={handleSubmit}>
        <h1>Employee & Task Management</h1>
        <p>Sign in to continue</p>

        {error && <div className="error-message">{error}</div>}

        <label>Email</label>
        <input
          type="email"
          value={email}
          onChange={(event) => setEmail(event.target.value)}
        />

        <label>Password</label>
        <input
          type="password"
          value={password}
          onChange={(event) => setPassword(event.target.value)}
        />

        <button type="submit" disabled={loading}>
          {loading ? "Signing in..." : "Sign In"}
        </button>
      </form>
    </div>
  );
}

export default LoginPage;