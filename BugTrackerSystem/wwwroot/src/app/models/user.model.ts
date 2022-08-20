export default interface User {
  id: string;
  name: string;
  email: string;
  role: string;
  numberOfContributedProjects: number;
  contributedProjects: any[];
  numberOfBugReports: number;
  bugReports: any[];
}
