import Aezakmi from './components/Aezakmi';
import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import TeacherControl from './components/Control/TeacherControl';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import LessonView from './components/Lessons/LessonView';
import QuizNew from './components/Quiz/QuizNew';
import QuizSolver from './components/Quiz/QuizSolver';
import SubjectView from './components/Subjects/SubjectView';

const AppRoutes = [
  {
    path: '/*',
    index: true,
    requireAuth: true,
    element: <Home />
  },
  {
    path: '/subject/:id',
      requireAuth: true,
      element: <SubjectView />
    },
    {
        path: '/aezakmi',
        requireAuth: true,
        element: <Aezakmi />
    },
    {
      path: '/lesson/:params',
      requireAuth: true,
      element: <LessonView />
    },
    {
      path: '/quiz/new/:params',
      requireAuth: true,
      element: <QuizNew />
    },
    {
      path: '/quiz_solver/:params',
      requireAuth: true,
      element: <QuizSolver />
    },
    {
      path: 'solved_quizzes/:id',
      requireAuth: true,
      element: <TeacherControl />
    },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
