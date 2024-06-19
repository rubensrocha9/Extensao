import { ExpenseManagerStatusType } from "../enum/expense-status.enum";

export interface ExpenseManager {
  id?: number;
  companyId?: number;
  Name?: string;
  Amount?: number;
  status?: ExpenseManagerStatusType;
  statusDescription?: string;
  creationDate?: DataTransfer;
  IsExpenseEmployee?: boolean;
}
