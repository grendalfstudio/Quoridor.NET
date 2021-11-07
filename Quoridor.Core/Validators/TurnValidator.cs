using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using Serilog;

namespace HavocAndCry.Quoridor.Core.Validators
{
    public static class TurnValidator
    {
        public static bool IsMoveValid(IGameField gameField, int row, int column, Player player)
        {
            if (gameField.Players.Any(p => p.Row == row && p.Column == column))
            {
                return false;
            }

            if (row < 0 || row > gameField.Size - 1 || column < 0 || column > gameField.Size - 1)
            {
                return false;
            }
            if ((player.Row != row && player.Column != column) 
                || player.Row - row == 2 
                || row - player.Row == 2
                || player.Column - column == 2
                || column - player.Column == 2)
            {
                return IsSpecialMoveValid(gameField, row, column, player);
            }

            return IsStandardMoveValid(gameField, row, column, player);
        }

        private static bool IsStandardMoveValid(IGameField gameField, int row, int column, Player player)
        {
            if (!IsPlayerOnCell(gameField, row, column) 
                && !IsWallBetweenCells(gameField, player.Row, player.Column, row, column))
            {
                return true;
            }
            return false;
        }

        private static bool IsSpecialMoveValid(IGameField gameField, int row, int column, Player player)
        {
            bool isNotJump = player.Row != row && player.Column != column;
            if (isNotJump)
            {
                bool isWallBlocksSituation = false;
                if (row > player.Row && column > player.Column)
                {
                    isWallBlocksSituation = gameField.IsWallAt(new WallCenter(player.Row, player.Column));
                }
                else if(row > player.Row && column < player.Column)
                {
                    isWallBlocksSituation = gameField.IsWallAt(new WallCenter(player.Row, column));
                }
                if (row < player.Row && column > player.Column)
                {
                    isWallBlocksSituation = gameField.IsWallAt(new WallCenter(row, player.Column));
                }
                else if(row < player.Row && column < player.Column)
                {
                    isWallBlocksSituation = gameField.IsWallAt(new WallCenter(row, column));
                }
                
                bool isSpecialColumnSituation = IsPlayerOnCell(gameField, row, player.Column)
                                                && !CanJumpOnCell(gameField, player.Row - (player.Row - row) * 2,
                                                    player.Column, player)
                                                && !isWallBlocksSituation;
                bool isSpecialRowSituation = IsPlayerOnCell(gameField, player.Row, column)
                                             && !CanJumpOnCell(gameField, player.Row,
                                                 player.Column - (player.Column - column) * 2, player)
                                             && !isWallBlocksSituation;
                
                if (!IsPlayerOnCell(gameField, row, column)
                    && ( isSpecialColumnSituation || isSpecialRowSituation))
                {
                    return true;
                }

                return false;
            }
            
            return CanJumpOnCell(gameField, row, column, player);
        }

        private static bool IsWallBetweenCells(IGameField gameField, int row1, int column1, int row2, int column2)
        {
            if (row1 == row2 && column1 < column2)
            {
                return (gameField.IsWallAt(new WallCenter(row2 - 1, column2 - 1), WallType.Vertical) ||
                        gameField.IsWallAt(new WallCenter(row2, column2 - 1), WallType.Vertical));
            }
             
            if (row1 == row2 && column1 > column2)
            {
                return (gameField.IsWallAt(new WallCenter(row2 - 1, column2), WallType.Vertical) ||
                        gameField.IsWallAt(new WallCenter(row2, column2), WallType.Vertical));
            }

            if (column1 == column2 && row1 > row2)
            {
                return (gameField.IsWallAt(new WallCenter(row2, column2 - 1), WallType.Horizontal) ||
                        gameField.IsWallAt(new WallCenter(row2, column2), WallType.Horizontal));
            }
            
            if (column1 == column2 && row1 < row2)
            {
                return (gameField.IsWallAt(new WallCenter(row2 - 1, column2 - 1), WallType.Horizontal) ||
                        gameField.IsWallAt(new WallCenter(row2 - 1, column2), WallType.Horizontal));
            }

            return false;
        }

        private static bool IsPlayerOnCell(IGameField gameField, int row, int column)
        {
            return gameField.Players.Any(p => p.Row == row && p.Column == column);
        }

        private static bool CanJumpOnCell(IGameField gameField, int row, int column, Player player)
        {
            if (player.Column != column && player.Row != row)
            {
                return false;
            }
            
            int specialCellRow = player.Row - (player.Row - row) / 2;
            int specialCellColumn = player.Column - (player.Column - column) / 2;
            
            if (!IsPlayerOnCell(gameField, row, column)
                && IsPlayerOnCell(gameField, specialCellRow, specialCellColumn)
                && !IsWallBetweenCells(gameField, specialCellRow, specialCellColumn, row, column))
            {
                return true;
            }

            return false;
        }

        public static bool IsWallValid(IGameField gameField, Wall newWall, int playerId, bool fromInput = false)
        {
            if (gameField.Players.First(p => p.PlayerId == playerId).WallsCount <= 0)
            {
                if(fromInput)
                {
                    Log.Information("Wall {@wall} is not valid, because player {id} don't have available walls", newWall, playerId);
                    
                    File.AppendAllText(@"./log.jsonc", $"//[{DateTime.Now}] Wall {newWall} is not valid, because player {playerId} don't have available walls\n\n");
                }                
                return false;
            }
            
            if (newWall.WallCenter.NorthRow < 0 
                || newWall.WallCenter.NorthRow > gameField.Size - 2
                || newWall.WallCenter.WestColumn < 0 
                || newWall.WallCenter.WestColumn > gameField.Size - 2)
            {
                if(fromInput)
                {
                    Log.Information("Wall {@wall} from player {id} is not valid, because it's coords outside of field",
                        newWall, playerId);
                    File.AppendAllText(@"./log.jsonc", $"//[{DateTime.Now}] Wall {newWall} from player {playerId} is not valid, because it's coords outside of field\n\n");
                }                
                return false;
            }

            if (gameField.IsWallAt(newWall.WallCenter))
            {
                if(fromInput)
                {
                    Log.Information("Wall {@wall} from player {id} is not valid, because there is already wall at this coords",
                        newWall, playerId);
                    File.AppendAllText(@"./log.jsonc", $"//[{DateTime.Now}] Wall {newWall} from player {playerId} is not valid, because there is already wall at this coords\n\n");
                }                
                return false;
            }
            
            if (newWall.Type == WallType.Horizontal
                && (gameField.IsWallAt(new WallCenter(newWall.WallCenter.NorthRow, newWall.WallCenter.WestColumn - 1), WallType.Horizontal)
                || gameField.IsWallAt(new WallCenter(newWall.WallCenter.NorthRow, newWall.WallCenter.WestColumn + 1), WallType.Horizontal)))
            {
                if(fromInput)
                {
                    Log.Information("Wall {@wall} from player {id} is not valid, because it overlaps with other horizontal wall",
                        newWall, playerId);
                    File.AppendAllText(@"./log.jsonc", $"//[{DateTime.Now}] Wall {newWall} from player {playerId} is not valid, because it overlaps with other horizontal wall\n\n");
                }                
                return false;
            }
            
            if (newWall.Type == WallType.Vertical
                && (gameField.IsWallAt(new WallCenter(newWall.WallCenter.NorthRow - 1, newWall.WallCenter.WestColumn), WallType.Vertical)
                || gameField.IsWallAt(new WallCenter(newWall.WallCenter.NorthRow + 1, newWall.WallCenter.WestColumn), WallType.Vertical)))
            {
                if(fromInput)
                {
                    Log.Information("Wall {@wall} from player {id} is not valid, because it overlaps with other vertical wall",
                        newWall, playerId);
                    File.AppendAllText(@"./log.jsonc", $"//[{DateTime.Now}] Wall {newWall} from player {playerId} is not valid, because it overlaps with other vertical wall\n\n");
                }                
                return false;
            }
            
            return true;
        }
    }
}