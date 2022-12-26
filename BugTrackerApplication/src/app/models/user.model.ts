import { BugReportData } from './bug.model';
import { ProjectData } from './project.model';

export default interface User {
  id: string;
  name: string;
  email: string;
  role: string;
  contributedProjects: ProjectData[];
  bugReports: BugReportData[];
}

export interface LoginResponse {
  token: string;
  user: AuthResponse;
}

export interface AuthResponse {
  id: string;
  name: string;
  email: string;
  role: string;
}

export interface UpsertUser {
  id?: string;
  name: string;
  email: string;
  currentPassword: string;
  newPassword: string;
  role?: string;
}

export interface DecodedUser {
  id: string;
  name: string;
  role: string;
}

export interface ContributorData {
  id: string;
  name: string;
}
