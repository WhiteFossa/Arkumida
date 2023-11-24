#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models.TextsStatistics;

namespace webapi.Mappers.Implementations;

public class TextsStatisticsEventsMapper : ITextsStatisticsEventsMapper
{
    private readonly ITextsMapper _textsMapper;
    private readonly ICreaturesMapper _creaturesMapper;

    public TextsStatisticsEventsMapper
    (
        ITextsMapper textsMapper,
        ICreaturesMapper creaturesMapper
    )
    {
        _textsMapper = textsMapper;
        _creaturesMapper = creaturesMapper;
    }
    
    public IReadOnlyCollection<TextsStatisticsEvent> Map(IEnumerable<TextsStatisticsEventDbo> statisticsEvents)
    {
        if (statisticsEvents == null)
        {
            return null;
        }

        return statisticsEvents.Select(se => Map(se)).ToList();
    }

    public TextsStatisticsEvent Map(TextsStatisticsEventDbo statisticsEvent)
    {
        if (statisticsEvent == null)
        {
            return null;
        }

        return new TextsStatisticsEvent()
        {
            Id = statisticsEvent.Id,
            Timestamp = statisticsEvent.Timestamp,
            Text = _textsMapper.Map(statisticsEvent.Text),
            Page = statisticsEvent.Page,
            Type = statisticsEvent.Type,
            CausedByCreature = _creaturesMapper.Map(statisticsEvent.CausedByCreature),
            Ip = statisticsEvent.Ip,
            UserAgent = statisticsEvent.UserAgent
        };
    }

    public TextsStatisticsEventDbo Map(TextsStatisticsEvent statisticsEvent)
    {
        if (statisticsEvent == null)
        {
            return null;
        }

        return new TextsStatisticsEventDbo()
        {
            Id = statisticsEvent.Id,
            Timestamp = statisticsEvent.Timestamp,
            Text = _textsMapper.Map(statisticsEvent.Text),
            Page = statisticsEvent.Page,
            Type = statisticsEvent.Type,
            CausedByCreature = _creaturesMapper.Map(statisticsEvent.CausedByCreature),
            Ip = statisticsEvent.Ip,
            UserAgent = statisticsEvent.UserAgent
        };
    }

    public IReadOnlyCollection<TextsStatisticsEventDbo> Map(IEnumerable<TextsStatisticsEvent> statisticsEvents)
    {
        if (statisticsEvents == null)
        {
            return null;
        }

        return statisticsEvents.Select(se => Map(se)).ToList();
    }
}