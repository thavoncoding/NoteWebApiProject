# Notes App Frontend

This is the **Vue 3 + TypeScript + Vite** frontend for the Notes App. It connects to the NotesApi backend and provides a modern, responsive UI for user authentication and note management.

## Features

- User registration and login (JWT-based)
- Create, view, edit, and delete notes
- Search and sort notes
- Responsive, modern UI (Tailwind CSS)
- Toast notifications

## Prerequisites

- [Node.js](https://nodejs.org/) (v18 or newer recommended)
- [npm](https://www.npmjs.com/) (comes with Node.js)
- The [NotesApi backend](../back-end/README.md) running (see backend README for setup)

## Project Setup

1. **Clone the repository**

   ```bash
   git clone <your-repo-url>
   cd front-end
   ```

2. **Install dependencies**

   ```bash
   npm install
   ```

3. **Configure API endpoint (optional)**

   By default, the frontend expects the backend API at `http://localhost:5280/api/`.  
   If your backend runs elsewhere, update the API URLs in your code (see `src/features/notes/NoteList.vue` and auth files).

4. **Start the development server**

   ```bash
   npm run dev
   ```

   The app will be available at [http://localhost:5173](http://localhost:5173) (or as shown in your terminal).

5. **Build for production**

   ```bash
   npm run build
   ```

6. **Preview the production build**

   ```bash
   npm run preview
   ```

## Tech Stack

- [Vue 3](https://vuejs.org/)
- [TypeScript](https://www.typescriptlang.org/)
- [Vite](https://vitejs.dev/)
- [Tailwind CSS](https://tailwindcss.com/)
- [Vue Router](https://router.vuejs.org/)
- [Axios](https://axios-http.com/)
- [Vue Toast Notification](https://github.com/ankurk91/vue-toast-notification)

## Notes

- **Authentication:**  
  After login, a JWT token is saved in `localStorage` and used for authenticated requests.
- **Routing:**  
  Auth-protected routes redirect to login if the user is not authenticated.
- **Styling:**  
  Tailwind CSS is used for all styling. You can customize styles in `src/style.css`.

## Troubleshooting

- If you see CORS errors, make sure the backend is running and CORS is enabled.
- If you change the backend port or URL, update the API URLs in the frontend code.

